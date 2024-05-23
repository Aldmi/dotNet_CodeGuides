using EventSourcing_DynamoDb;
using EventSourcing_DynamoDb_V2;
using EventSourcing_DynamoDb.Events;

Console.WriteLine("EventSourcing");
var studentDatabase= new StudentDatabaseDynamoDb("http://localhost:8000");

var studentId = new Guid("948CCE45-86C7-4474-B7A1-83D65791D57C");

//-------------------------------------------------
// //событие создания студента
// var studentCreated = new StudentCreated
// {
//     StudentId = studentId,
//     Email = "aldmi@bk.ru",
//     FullName = "Alex Dmitr",
//     DateOfBirth = new DateTime(1989, 2, 23)
// };
// await studentDatabase.AppendAsync(studentCreated);
//
// //событие зачисление студента
// var studentEnrolled = new StudentEnrolled
// {
//     StudentId = studentId,
//     CourseName = "Math magister"
// };
// await studentDatabase.AppendAsync(studentEnrolled);
//
// //событие Обновления информации о студенте
// var studentUpdated= new StudentUpdated
// {
//     StudentId = studentId,
//     Email = "newEmail@bk.ru",
//     FullName = "Alex Dmitr",
// };
// await studentDatabase.AppendAsync(studentUpdated);


var student=await studentDatabase.GetStudentAsync(studentId);

Console.ReadKey();