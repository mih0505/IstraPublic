using System;
using System.Data.Entity;

namespace Istra.Entities
{    
    public class IstraContext : DbContext
    {
        static IstraContext()
        {
            Database.SetInitializer<IstraContext>(null);
        }
        public IstraContext()
            : base("IstraDb")
        { }

        public DbSet<Cause> Causes { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Housing> Housings { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Month> Months { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Study> Studies { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Activity> ActivityGroups { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<TypePayment> TypePayments { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateRate> TemplateRates { get; set; }
        public DbSet<TemplateGroup> TemplateGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ManageRole> ManageRoles { get; set; }
        public DbSet<Retention> Retentions { get; set; }
        public DbSet<Base> Bases { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<TypeOfTransaction> TypeOfTransactions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<AccessGroups> AccessGroups { get; set; }
    }
}
