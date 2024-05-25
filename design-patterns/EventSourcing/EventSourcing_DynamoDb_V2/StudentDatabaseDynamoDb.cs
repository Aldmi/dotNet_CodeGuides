using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using EventSourcing_DynamoDb_V2.Entities;
using EventSourcing_DynamoDb_V2.Events;
using Document = Amazon.DynamoDBv2.DocumentModel.Document;

namespace EventSourcing_DynamoDb_V2;

public class StudentDatabaseDynamoDb
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private const string TableName = "students";

    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        AllowOutOfOrderMetadataProperties = true //обязательно включать если $type находится не первым типом в Json строке
    };
    
    public StudentDatabaseDynamoDb(string serviceUrl)
    {
        var clientConfig = new AmazonDynamoDBConfig {ServiceURL =serviceUrl};
        var creds = new BasicAWSCredentials("xxx", "xxx");
        _amazonDynamoDb= new AmazonDynamoDBClient(creds, clientConfig);
    }


    
    public async Task AppendAsync<T>(T @event) where T : Event
    {
        @event.CreatedAtUtc = DateTime.UtcNow;
        var eventAsJson = JsonSerializer.Serialize<Event>(@event);
        var eventAsDocument = Document.FromJson(eventAsJson);
        var eventAsAttribute = eventAsDocument.ToAttributeMap();
        
        var studentView = await GetStudentAsync(@event.StreamId) ?? new Student();
        studentView.Apply(@event);
        var studentAsJson = JsonSerializer.Serialize(studentView);
        var studentAsDocument = Document.FromJson(studentAsJson);
        var studentAsAttribute = studentAsDocument.ToAttributeMap();
        
        var transactionRequest = new TransactWriteItemsRequest()
        { 
           //Транзакция по сохранению event и прекции studentView. (храним в одной табюлице, можно хранить в разных)
          TransactItems = 
          [
              new TransactWriteItem()
              {
                  Put = new Put
                  {
                      TableName = TableName,
                      Item = eventAsAttribute
                  }
              },
              new TransactWriteItem()
              {
                  Put = new Put
                  {
                      TableName = TableName,
                      Item = studentAsAttribute
                  }
              }
          ]
        };
        
        await _amazonDynamoDb.TransactWriteItemsAsync(transactionRequest);
    }


    /// <summary>
    /// Получить проекцию студента из БД 
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns></returns>
    public async Task<Student?> GetStudentAsync(Guid studentId)
    {
        var request = new GetItemRequest()
        {
          TableName = TableName,
          Key= new Dictionary<string, AttributeValue>()
          {
              {"pk", new AttributeValue {S = $"{studentId.ToString()}_view"}},
              {"sk", new AttributeValue {S = $"{studentId.ToString()}_view"}}
          }
        };
    
        var response = await _amazonDynamoDb.GetItemAsync(request);
        if (response.Item.Count == 0)
        {
            return null;
        }
    
        var studentAsDocument = Document.FromAttributeMap(response.Item);
        var studentAsJson = studentAsDocument.ToJson();
        var student = JsonSerializer.Deserialize<Student>(studentAsJson);
        return student;
    }
    
    

    // public async Task<Student?> GetStudentAsync(Guid studentId)
    // {
    //     var request = new QueryRequest
    //     {
    //         TableName = TableName,
    //         KeyConditionExpression = "pk = :v_Pk",
    //         ExpressionAttributeValues = new Dictionary<string, AttributeValue>
    //         {
    //             {":v_Pk", new AttributeValue {S = studentId.ToString()}}
    //         }
    //     };
    //
    //     var response = await _amazonDynamoDb.QueryAsync(request);
    //     if (response.Count == 0)
    //     {
    //         return null;
    //     }
    //
    //     var itemsAsDocuments = response.Items.Select(item=> Document.FromAttributeMap(item));
    //     var studentEvents = itemsAsDocuments
    //         .Select(document =>
    //         {
    //             var json = document.ToJson();
    //             return JsonSerializer.Deserialize<Event>(json, SerializerOptions);
    //         })
    //         .OrderBy(e=>e!.CreatedAtUtc);
    //
    //
    //     var student = new Student();
    //     foreach (var studentEvent in studentEvents)
    //     {
    //         student.Apply(studentEvent!);
    //     }
    //     
    //     return student;
    // }
    
}