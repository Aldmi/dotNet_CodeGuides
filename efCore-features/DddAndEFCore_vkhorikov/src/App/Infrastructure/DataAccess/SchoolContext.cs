using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.DataAccess
{
    public sealed class SchoolContext : DbContext
    {
        
        private readonly string _connectionString;
        private readonly bool _useConsoleLogger;
        private readonly EventDispatcher _eventDispatcher;

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public SchoolContext(string connectionString, bool useConsoleLogger, EventDispatcher eventDispatcher)
        {
            _connectionString = connectionString;
            _useConsoleLogger = useConsoleLogger;
            _eventDispatcher = eventDispatcher;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                    .AddConsole();
            });

            optionsBuilder
                .UseNpgsql(_connectionString)
                .UseLazyLoadingProxies();

            if (_useConsoleLogger)
            {
                optionsBuilder
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(x =>
            {
                x.ToTable("Student").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("StudentID").UseHiLo();
                x.Property(p => p.Email).HasConversion(p => p.Value, p => Email.Create(p).Value);
                
                x.OwnsOne(p => p.Name, p =>
                {
                    p.Property(pp => pp.First);
                    p.Property(pp => pp.Last);
                    p.HasOne(pp => pp.Suffix).WithMany().OnDelete(DeleteBehavior.Restrict);
                });
                
                x.HasOne(p => p.FavoriteCourse).WithMany();

                x.HasMany(p => p.Enrollments).WithOne(p => p.Student)
                    .OnDelete(DeleteBehavior.Cascade)
                    .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            modelBuilder.Entity<Suffix>(x =>
            {
                x.ToTable("Suffix").HasKey(p => p.Id);
                x.Property(p => p.Id).HasColumnName("SuffixID").UseHiLo();
                x.Property(p => p.Name);
            });
            modelBuilder.Entity<Course>(x =>
            {
                x.ToTable("Course").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("CourseID"); //UseHiLo не используем, т.к. инициализируем БД, нчальными значениями Course, где генерация ключа идет на стороне БД, и при использовании UseHiLo, возникает конфиликт ключей (попытка добавить id=1, а ключ уже есть.)
                x.Property(p => p.Name);
            });
            modelBuilder.Entity<Enrollment>(x =>
            {
                x.ToTable("Enrollment").HasKey(k => k.Id);
                x.Property(p => p.Id).HasColumnName("EnrollmentID").UseHiLo();
                
                //Не обязательно явно задавать----------------
                x.HasOne(p => p.Student).WithMany(p => p.Enrollments);
                x.HasOne(p => p.Course).WithMany();//.HasForeignKey("CourseSuperId"); можно явно задавать имя ключа навигации (если есть несколько связей на Course, для каждого из них нужен свой ключ)
                //-------------------------------------------
                
                x.Property(p => p.Grade);
            });

            SeedData(modelBuilder);
        }


        private void SeedData(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Course>().HasData(Course.AllCourses.ToList());
            modelBuilder.Entity<Suffix>().HasData(Suffix.AllSuffixes.ToList());
        }

        
        public override int SaveChanges()
        {
            DispatchDomainEventsAsync();
            int result = base.SaveChanges();
            return result;
        }
        
        private void DispatchDomainEventsAsync()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
            
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
            
            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            _eventDispatcher?.Dispatch(domainEvents);
        }
    }
}
