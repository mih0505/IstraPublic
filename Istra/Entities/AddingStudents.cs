using System;

namespace Istra
{
    public class AddingStudents
    {
        public int StudentId { get; set; }
        public int? SchoolId { get; set; }
        public int StatusId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Status { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }        
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string School { get; set; }
        public int? Class { get; set; }
        public string Shift { get; set; }
        public string Note { get; set; }
    }
}