using System.Threading.Channels;
using Streamiz.Kafka.Net;
using Streamiz.Kafka.Net.SerDes;
using Streamiz.Kafka.Net.Stream;
using Streamiz.Kafka.Net.Table;

Console.WriteLine("Starting...");

const string ORDER_TOPIC = "orders";
const string CUSTOMER_TOPIC = "customers";
const string PRODUCT_TOPIC = "products";
const string CUSTOMER_STORE = "customer-store";
const string PRODUCT_STORE = "product-store";
const string ENRICHED_ORDER_TOPIC = "orders-enriched";

CancellationTokenSource source = new();
string boostrapserver = "localhost:29092";

var config = new StreamConfig
{
    ApplicationId = "shoes-shop-app",
    ClientId = "shoes-shop-app-client",
    // Where to find Kafka broker(s).
    BootstrapServers = boostrapserver,
    // Set to earliest so we don't miss any data that arrived in the topics before the process
    // started
    AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest
};

var t = GetTopology();

KafkaStream stream = new(t, config);

Console.CancelKeyPress += (o, e) => { source.Cancel(); };


try
{
    await stream.StartAsync(source.Token);
}
catch (Exception e)
{
    Console.WriteLine(e);
}


static Topology GetTopology()
{
    StreamBuilder builder = new();

    var orderStream = builder.Stream<string, Order, StringSerDes, JsonSerDes<Order>>(ORDER_TOPIC);
    
    var customers = builder.GlobalTable<string, Customer, StringSerDes, JsonSerDes<Customer>>(
        CUSTOMER_TOPIC, RocksDb.As<string, Customer>(CUSTOMER_STORE));

    var products = builder.GlobalTable<string, Product, StringSerDes, JsonSerDes<Product>>(
        PRODUCT_TOPIC, RocksDb.As<string, Product>(PRODUCT_STORE));

    var customerOrderStream = orderStream.Join(customers,
            (orderId, order) => order.customer_id,
            (order, customer) => new CustomerOrder(customer, order));

    var enrichedOrderStream = customerOrderStream.Join(products,
            (orderId, customerOrder) => customerOrder.Order.product_id,
            (customerOrder, product) => EnrichedOrderBuilder.Build(customerOrder, product));

    enrichedOrderStream.To<StringSerDes, JsonSerDes<EnrichedOrder>>(ENRICHED_ORDER_TOPIC);

    return builder.Build();
}