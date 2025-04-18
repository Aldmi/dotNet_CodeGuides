using EventSourcingMarten;
using Marten;
using Marten.Events.Projections;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddMarten(options =>
	{
		options.Connection("Server=localhost;Port=5433;Database=eventsourcing_marten;User Id=postgres;Password=dmitr;");
		options.UseSystemTextJsonForSerialization(); //Хранение объектов в JSON (в PG есть тип Json - хранения будет оптимизированно)
		options.Projections.Add<OrderProjection>(ProjectionLifecycle.Inline); //ProjectionLifecycle.Inline - сохраняет проекцию в той же транзакции что и сохранение нового события
		if (builder.Environment.IsDevelopment())
		{
			options.AutoCreateSchemaObjects = AutoCreate.All; //Создать все схемы в БД
		}
	});
	

var app = builder.Build();

app.MapGet("orders/{orderId:guid}", async (Guid orderId, IQuerySession session) =>
{
	//var order = await session.Events.AggregateStreamAsync<Order>(orderId);  //Обраьатывает поток и агрегирует все события к объекту Order
	var order = await session.LoadAsync<Order>(orderId);  //Загружаем данные из проекции
	return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.MapGet("orders", async (IQuerySession session)=>
{
	var orders = await session.Query<Order>().ToListAsync();
	return Results.Ok(orders);
});


//--------------------------------------------------------------------------------------------------
//Создать заказ
app.MapPost("orders", async (CreateOrderRequest request, IDocumentStore store) =>
{
	var orderCreated = new Events.OrderCreated
	{
		ProductName = request.ProductName,
		DeliveryAddress = request.DeliveryAddress
	};
	await using var session = store.LightweightSession();
	session.Events.StartStream<Order>(orderCreated.Id, orderCreated);
	await session.SaveChangesAsync();
	return Results.Ok(orderCreated);
});

//Добавить адрес доставки
app.MapPost("orders/{orderId:guid}/address", async (Guid orderId, DeliveryAddressUpdateRequest request, IDocumentStore store) =>
{
	var addressUpdated = new Events.OrderAddressUpdated
	{
		Id = orderId,
		DeliveryAddress = request.DeliveryAddress
	};
	await using var session = store.LightweightSession();
	session.Events.Append(addressUpdated.Id, addressUpdated); //Добавить событие
	await session.SaveChangesAsync();
	return Results.Ok(addressUpdated);
});

//Подтвердить статус заказа
app.MapPost("orders/{orderId:guid}/dispatch", async (Guid orderId, IDocumentStore store) =>
{
	var orderDispatched = new Events.OrderDispatched()
	{
		Id = orderId,
		DispatchedAtUtc = DateTime.UtcNow
	};
	await using var session = store.LightweightSession();
	session.Events.Append(orderDispatched.Id, orderDispatched); //Добавить событие
	await session.SaveChangesAsync();
	return Results.Ok(orderDispatched);
});

//отправить в доставку
app.MapPost("orders/{orderId:guid}/outfordelivery", async (Guid orderId, IDocumentStore store) =>
{
	var orderOutForDelivery = new Events.OrderOutForDelivery()
	{
		Id = orderId,
		OutForDeliveryAtUtc = DateTime.UtcNow
	};
	await using var session = store.LightweightSession();
	session.Events.Append(orderOutForDelivery.Id, orderOutForDelivery); //Добавить событие
	await session.SaveChangesAsync();
	return Results.Ok(orderOutForDelivery);
});

//Доставлен заказ
app.MapPost("orders/{orderId:guid}/delivererd", async (Guid orderId, IDocumentStore store) =>
{
	var orderDelivered = new Events.OrderDelivered
	{
		Id = orderId,
		DeliveredAtUtc = DateTime.UtcNow
	};
	await using var session = store.LightweightSession();
	session.Events.Append(orderDelivered.Id, orderDelivered); //Добавить событие
	await session.SaveChangesAsync();
	return Results.Ok(orderDelivered);
});


app.Run();

