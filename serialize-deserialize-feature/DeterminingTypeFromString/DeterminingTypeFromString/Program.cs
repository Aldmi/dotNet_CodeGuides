using System.Globalization;
using System.Text.Json;
using DeterminingTypeFromString.Messages;

//Получаю JSON объект и его тип. 
var customerCreatedJson = """
                          {
                            "FullName": "Aldmi",
                            "Email": "Aldmi@bk.ru",
                            "GitHubUserName": "Aldmi_123",
                            "DateOfBirth":"2019-08-01"
                          }
                          """;
 string messageType = "CustomerCreated";


 customerCreatedJson = """
                          {
                            "Id": "EF031D43-450C-49BF-9465-EA0C489611BC"
                          }
                          """;
messageType = "CustomerDeleted";



var type = Type.GetType($"DeterminingTypeFromString.Messages.{messageType}");
if (type is null)
{
    Console.WriteLine($"Unknown message type {messageType}");
}
else
{
    var typedMessage = (IMessage)JsonSerializer.Deserialize(customerCreatedJson, type)!;
    //(PUB-SUB) -typedMessage можно отправлять например на Mediatr.
    //await mediatr.Send(typedMessage);
    Console.WriteLine(typedMessage.ToString());
}





