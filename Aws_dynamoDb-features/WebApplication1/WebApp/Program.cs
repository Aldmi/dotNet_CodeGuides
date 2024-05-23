using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

using WebApp.DbModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var dynamoDbConfig = builder.Configuration.GetSection("DynamoDb");
var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

//Используем локальную версию для разработки и версию AWS для production.
if (runLocalDynamoDb)
{
    builder.Services.AddSingleton<IAmazonDynamoDB>(_ =>
    {
        var clientConfig = new AmazonDynamoDBConfig {ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl")};
        var creds = new BasicAWSCredentials("xxx", "xxx");
        return new AmazonDynamoDBClient(creds, clientConfig);
    });
}
else
{
    builder.Services.AddAWSService<IAmazonDynamoDB>();
}

builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/RawPutItem", async (IAmazonDynamoDB dynamoDb) =>
    {
        var request = new PutItemRequest
        {
            TableName = "Test_2",
            Item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue {N = "1"}},
                {"Title", new AttributeValue {S = "Title_1"}}
            }
        };
        await dynamoDb.PutItemAsync(request);
        return 0;
    })
    .WithOpenApi();


//-----------------------------------------------------------------
//Использование IDynamoDBContext


app.MapGet("cars/GetAll", async (IDynamoDBContext dbContext) =>
{
    var cars = await dbContext.ScanAsync<CarDbModel>(default).GetRemainingAsync();
    if (cars.Count == 0)
        return Results.NotFound(new {message = "Таблица пуста"});

    return Results.Json(cars);
});


app.MapGet("cars/GetById/{id}", async (int id, IDynamoDBContext dbContext) =>
{
    var car = await dbContext.LoadAsync<CarDbModel>(id);
    if (car == null)
        return Results.NotFound(new {message = "объект не найден"});

    return Results.Json(car);
});


app.MapPost("cars/AddItem", async ([FromBody] CarDbModel carDto, IDynamoDBContext dbContext) =>
{
    //Если Id уже есть в БД, то перепишем объект
    await dbContext.SaveAsync(carDto);
    return Results.Json(carDto);
});


app.MapDelete("/cars/deleteById/{id}", async (int id, IDynamoDBContext dbContext) =>
{
// получаем пользователя по id
   
    var car = await dbContext.LoadAsync<CarDbModel>(id);
    if (car == null)
        return Results.NotFound(new {message = "объект не найден"});
    
    await dbContext.DeleteAsync(car);
    return Results.Json(car);
});

app.MapPut("cars/UpdateItem", async ([FromBody]CarDbModel carDto, IDynamoDBContext dbContext) => {
    
    var car = await dbContext.LoadAsync<CarDbModel>(carDto.Id);
    if (car == null)
        return Results.NotFound(new {message = "объект не найден"});
    
    await dbContext.SaveAsync(carDto);
    return Results.Json(carDto);
});


app.Run();