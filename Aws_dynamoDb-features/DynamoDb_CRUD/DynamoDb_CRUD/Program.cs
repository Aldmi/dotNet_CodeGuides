
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

Console.WriteLine("DynamoDb_CRUD");

AmazonDynamoDBConfig amazonDynamoDbConfig = new AmazonDynamoDBConfig
{
    ServiceURL = "http://localhost:8000",
};

//Для локального запуска нужно указать BasicAWSCredentials с любыми значениями Accesskey и SecretKey
var creds = new BasicAWSCredentials("xxx", "xxx");
AmazonDynamoDBClient dynamoDb = new AmazonDynamoDBClient(creds, amazonDynamoDbConfig);

var tableName = "ProductCatalog";
var request = new PutItemRequest
{
    TableName = tableName,
    Item = new Dictionary<string, AttributeValue>
    {
        { "Id", new AttributeValue { N = "201" }},
        { "Title", new AttributeValue { S = "Book 201 Title" }},
        { "ISBN", new AttributeValue { S = "11-11-11-11" }},
        { "Price", new AttributeValue { S = "22.00" }},
        {
            "Authors", new AttributeValue { SS = ["Author1", "Author2"]}
        }
    }
};
var response= await dynamoDb.PutItemAsync(request);

Console.ReadKey();


