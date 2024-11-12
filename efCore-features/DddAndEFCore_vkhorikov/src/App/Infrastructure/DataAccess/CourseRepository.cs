using App.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public sealed class CourseRepository
    {
        private readonly SchoolContext _context;
        public CourseRepository(SchoolContext context)
        {
            _context = context;
        }

        public Course GetById(long courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null)
                return null;
            
            return course;
        }

        public void Save(Course course)
        {
            _context.Courses.Add(course);
        }
    }
}
