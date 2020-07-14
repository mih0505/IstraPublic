using System.Collections.Generic;

namespace Istra.Entities
{
    public class Worker
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string LastnameEn { get; set; }
        public string FirstnameEn { get; set; }
        public string MiddlenameEn { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Dismissed { get; set; }//уволенный
        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public bool IsRemoved { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int? PostId { get; set; }
        public Post Post { get; set; }
        public bool AllAccessGroups { get; set; }


        public string LastnameFM
        {
            get { return Lastname + " " + Firstname.Substring(0, 1) + "." + Middlename.Substring(0, 1) + "."; }
        }

        public string Fullname
        {
            get { return Lastname + " " + Firstname + " " + Middlename; }
        }

        public List<Enrollment> Enrollments { get; set; }
        public List<Enrollment> Exclusions { get; set; }
        public List<Group> Groups { get; set; }
        public List<Student> Students { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Payment> RemovedWorker { get; set; }
        public List<Retention> Retentions { get; set; }
        public Worker()
        {
            Enrollments = new List<Enrollment>();
            Groups = new List<Group>();
            Students = new List<Student>();
            Lessons = new List<Lesson>();
            Schedules = new List<Schedule>();
            Payments = new List<Payment>();
            Retentions = new List<Retention>();
        }




    }
}