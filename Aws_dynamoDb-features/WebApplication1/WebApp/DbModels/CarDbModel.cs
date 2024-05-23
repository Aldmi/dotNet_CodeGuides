using Amazon.DynamoDBv2.DataModel;

namespace WebApp.DbModels;

/// <summary>
/// Модель данных для хранения.
/// БД cars должна быть созданна заранее (id ключ Number типа)
/// </summary>
[DynamoDBTable("cars")]
public class CarDbModel
{
    [DynamoDBHashKey("id")]
    public required int? Id { get; set; }
    
    [DynamoDBProperty("name")]
    public required string? Name { get; set; }

    [DynamoDBProperty("car_type")]
    public required CarTypeDbModel CarType { get; set; }
}

//Сериализует автоматически в JSON для хранения
public class CarTypeDbModel
{
    public required string Name { get; set; }
}