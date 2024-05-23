using EventSourcing_InMemDb;
using EventSourcing_InMemDb.Events;

Console.WriteLine("EventSourcing");
var studentDatabase= new StudentDatabaseInMem();

var studentId = new Guid("948CCE45-86C7-4474-B7A1-83D65791D57C");

//-------------------------------------------------
//событие создания студента
var studentCreated = new StudentCreated
{
    StudentId = studentId,
    Email = "aldmi@bk.ru",
    FullName = "Alex Dmitr",
    DateOfBirth = new DateTime(1989, 2, 23)
};
studentDatabase.Append(studentCreated);

//событие зачисление студента
var studentEnrolled = new StudentEnrolled
{
    StudentId = studentId,
    CourseName = "Math magister"
};
studentDatabase.Append(studentEnrolled);

//событие Обновления информации о студенте
var studentUpdated= new StudentUpdated
{
    StudentId = studentId,
    Email = "newEmail@bk.ru",
    FullName = "Alex Dmitr",
};
studentDatabase.Append(studentUpdated);


//-------------------------------------------------
//Получить студента из БД

var student = studentDatabase.GetStudent(studentId);
var studentFromView = studentDatabase.GetStudentView(studentId);

Console.ReadKey();








