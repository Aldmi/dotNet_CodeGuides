using System.Collections.Generic;
using System.Linq;
using App.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public sealed class StudentRepository
    {
        private readonly SchoolContext _context;
        public StudentRepository(SchoolContext context)
        {
            _context = context;
        }

        public List<Student> GetAll()
        {
            return _context.Students
                .Include(s=>s.Enrollments)
                .Include(s=>s.FavoriteCourse)
                .ToList();
        }

        public Student GetById(long studentId)
        {
            Student student = _context.Students.Find(studentId);

            if (student == null)
                return null;

            _context.Entry(student).Collection(x => x.Enrollments).Load();
            return student;
        }

        public void Save(Student student)
        {
            _context.Students.Add(student); //после добавления автоматом присваимается Id, т.к. настренна генерация ключей на клиенте через UseHiLo.
        }
    }
}
