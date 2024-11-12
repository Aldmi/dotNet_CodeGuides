using App.Infrastructure.DataAccess;

namespace App
{
    public class CourseController
    {
        private readonly SchoolContext _context;
        private readonly CourseRepository _repository;

        public CourseController(SchoolContext context)
        {
            _context = context;
            _repository = new CourseRepository(context);
        }
        
        public string AddCourse(string courseName)
        {
            Course course = Course.CreateNew(courseName);
            if (course == null)
                return "Course not Create";
            
            _repository.Save(course);
            var res=_context.SaveChanges();

            return "OK";
        }
    }
}
