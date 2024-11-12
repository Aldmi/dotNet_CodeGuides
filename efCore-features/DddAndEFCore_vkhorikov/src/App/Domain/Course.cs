using System.Linq;

namespace App
{
    public class Course : Entity
    {
        public static readonly Course Calculus = new Course(1, "Calculus");
        public static readonly Course Chemistry = new Course(2, "Chemistry");
        public static readonly Course Literature = new Course(3, "Literature");
        public static readonly Course Trigonometry = new Course(4, "Trigonometry");
        public static readonly Course Microeconomics = new Course(5, "Microeconomics");
    
        public static readonly Course[] AllCourses = { Calculus, Chemistry, Literature, Trigonometry, Microeconomics };
    
        public string Name { get; }
    
        protected Course()
        {
        }
    
        private Course(long id, string name) : base(id)
        {
            Name = name;
        }
        
        public static Course FromId(long id)
        {
            return AllCourses.SingleOrDefault(x => x.Id == id);
        }
        
        public static Course CreateNew(string name)
        {
            return new Course(0, name);
        }
    }
}
