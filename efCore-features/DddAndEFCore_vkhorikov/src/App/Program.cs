using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using App.Infrastructure.DataAccess;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App
{
    public class Program
    {
        public static void Main()
        {
            var result = CreateDb();
            if (result.IsFailure)
            {
                Console.WriteLine(result.Error);
                return;
            }
            
            //Command for Courses----------------------------------------------
            string resultCourse1 = Execute(x => x.AddCourse("nuclear phisical_666"));
            
            //Command for Students---------------------------------------------
            string resultRegister1 = Execute(x => x.RegisterStudent("Alex", "Alex@gmail.com", 1, "ggggg@Mail.ru",4, Grade.A));
            //string resultRegister2 = Execute(x => x.RegisterStudent("Carl", "carl@gmail.com", 2, "dd@Mail.ru",1, Grade.B));
            
            string result2 = Execute(x => x.EnrollStudent(1, 2, Grade.A));
            
            string result5 = Execute(x => x.EditPersonalInfo(1, "Carl 2", "Carlson 2", 2, "carl1@gmail.com", 1));
            //string result3 = Execute(x => x.DisenrollStudent(1, 4));
            //string result = Execute(x => x.CheckStudentFavoriteCourse(1, 2));
            
            
            //Queries for Students--------------------------------------------
            var studentList = Execute(x => x.GetAll());
            var first = studentList.FirstOrDefault();
            var v= first?.FavoriteCourse;

            Console.WriteLine($"App Started processId= '{Process.GetCurrentProcess().Id}'" );
            Console.ReadKey();
        }

        
        private static Result CreateDb()
        {
            return Result.Try(() =>
            {
                string connectionString = GetConnectionString();
                using var context = new SchoolContext(connectionString, true, null);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            });
        }
        
        
        private static string Execute(Func<StudentController, string> func)
        {
            string connectionString = GetConnectionString();
            IBus bus = new Bus();
            var messageBus = new MessageBus(bus);
            var eventDispatcher = new EventDispatcher(messageBus);

            using var context = new SchoolContext(connectionString, true, eventDispatcher);
            var controller = new StudentController(context);
            return func(controller);
        }
        
        
        private static List<Student> Execute(Func<StudentController, List<Student>> func)
        {
            string connectionString = GetConnectionString();
            IBus bus = new Bus();
            var messageBus = new MessageBus(bus);
            var eventDispatcher = new EventDispatcher(messageBus);

            using var context = new SchoolContext(connectionString, true, eventDispatcher);
            var controller = new StudentController(context);
            
             var list= func(controller);
            // var first = list.First();
            // var enrollments = first.Enrollments.ToArray(); //Lazy loading работает только в пределах контекста.
            return list;
        }
        
        
        private static string Execute(Func<CourseController, string> func)
        {
            string connectionString = GetConnectionString();
            IBus bus = new Bus();
            var messageBus = new MessageBus(bus);
            var eventDispatcher = new EventDispatcher(messageBus);

            using var context = new SchoolContext(connectionString, true, eventDispatcher);
            var controller = new CourseController(context);
            return func(controller);
        }

        
        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration["ConnectionString"];
        }
    }
}
