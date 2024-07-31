using EfCoreAdvanced.Database;
using EfCoreAdvanced.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EfCoreAdvanced.Endpoints;

public static class RegiserStudentEndpoint
{
    public static WebApplication AddStudentEndpoints(this WebApplication app)
    {
        app.MapPost("/registerStudent", (
                string firstName,
                string lastName,
                long favoriteCourseId,
                Grade favoriteCourseGrade,
                [FromServices] ApplicationDbContext dbContext
            ) =>
            {
                var nameResult = Name.Create(firstName, lastName);
                if (nameResult.IsFailure)
                    return Results.BadRequest(nameResult.Error);

                var courseResult = Course.FromId(favoriteCourseId);
                if (courseResult.IsFailure)
                    return Results.BadRequest(courseResult.Error);

                //Создадим нового студента
                var student = new Student(nameResult.Value, courseResult.Value);

                //зачислим студента на курс
                var enrollment = new Enrollment(courseResult.Value, student, favoriteCourseGrade);
                student.Enrollments.Add(enrollment);
                
                //dbContext.Students.Add(student); //ERROR: все новые сущности помечаются как "Added". Поскольку courseResult извлекаем НЕ из БД, в трекере ее нет (она не отслеживается) и считается что она новая, хотя она уже была добавлденна в БД
                //При добавлении новой сущности лутше использовать Attach метод, вместо Add
                dbContext.Students.Attach(student); //Используя Attach трекер смотрит Id сущности, если она не выставленна то состояние - "Added". Если выставленно, то "Unchanged". Т.е. для course выставися "Unchanged".

                //DEBUG. для отладки отслеживать состояние ChangeTracker
                var entries = dbContext.ChangeTracker.Entries().ToList();

                dbContext.SaveChangesAsync();

                return Results.Ok("Ok");
            })
            .WithName("RegisterStudent")
            .WithOpenApi();


        
        app.MapPut("/edit", async (
                long studentId,
                string firstName,
                string lastName,
                long favoriteCourseId,
                [FromServices] ApplicationDbContext dbContext
            ) =>
            {
                var student = await dbContext.Students.FindAsync(studentId);//FindAsync а не FirstOrDefault() потому что FindAsync извлекает данрные из трекера (если их там нет то из БД), т.е. для повторных запросов оптимальнее
                if(student is null)
                    return Results.BadRequest("Student not Found");
                
                var nameResult = Name.Create(firstName, lastName);
                if (nameResult.IsFailure)
                    return Results.BadRequest(nameResult.Error);
                
                var courseResult = Course.FromId(favoriteCourseId);
                if (courseResult.IsFailure)
                    return Results.BadRequest(courseResult.Error);

                student.Name = nameResult.Value;
                student.FavoriteCourse = courseResult.Value;

                //DEBUG. для отладки отслеживать состояние ChangeTracker
                var entries = dbContext.ChangeTracker.Entries().ToList();
                
                await dbContext.SaveChangesAsync();
                
                return Results.Ok("Ok");
            })
            .WithName("edit")
            .WithOpenApi();
        
        
        
        app.MapPut("/enroll", async (
                long studentId,
                long courseId,
                Grade grade,
                [FromServices] ApplicationDbContext dbContext
            ) =>
            {
                //Загрузим сущность с необходимыми зависмостями (Выполнится JOIN по всем сущностям)
                var student = await dbContext
                    .Students
                    .Include(s => s.Enrollments).ThenInclude(enrollment => enrollment.Course)
                    .FirstOrDefaultAsync(s=>s.Id == studentId);
                
                if(student is null)
                    return Results.BadRequest("Student not Found");
                
                var course = await dbContext.Courses.FindAsync(courseId); 
                if (course is null)
                    return Results.BadRequest("Course not found");
                
                //Создать новое зачисление
                var enrollment = new Enrollment(course, student, grade);
                
                if(student.Enrollments.Any(e=>e.Course == course)) //Для выполнепния этого условия нужны зависимости (Include) у student
                    return Results.BadRequest("Enrollment already exist");
                
                //Добавить студенту новое зачисление
                student.Enrollments.Add(enrollment);
                
                //DEBUG. для отладки отслеживать состояние ChangeTracker
                var entries = dbContext.ChangeTracker.Entries().ToList();
                
                await dbContext.SaveChangesAsync();
                
                return Results.Ok("Ok");
            })
            .WithName("enroll")
            .WithOpenApi();
        
        
        
        app.MapDelete("/disenroll", async (
                long studentId,
                long courseId,
                [FromServices] ApplicationDbContext dbContext
            ) =>
            {
                var student = await dbContext
                    .Students
                    .Include(s => s.Enrollments).ThenInclude(enrollment => enrollment.Course)
                    .FirstOrDefaultAsync(s=>s.Id == studentId);
                
                if(student is null)
                    return Results.BadRequest("Student not Found");
                
                //Создать новое зачисление
                var enrollment = student.Enrollments.FirstOrDefault(e => e.Course.Id == courseId);
                if (enrollment is null)
                    return Results.BadRequest("Enrollment not found");
                
                student.Enrollments.Remove(enrollment);
                
                //DEBUG. для отладки отслеживать состояние ChangeTracker
                var entries = dbContext.ChangeTracker.Entries().ToList();
                
                await dbContext.SaveChangesAsync();
                
                return Results.Ok("Ok");
            })
            .WithName("disenroll")
            .WithOpenApi();

        return app;
    }
}