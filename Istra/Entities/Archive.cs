using System;
using Istra.Entities;

namespace Istra
{
    public class Archive
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public int? SchoolId { get; set; }
        public int EnrollId { get; set; }
        public int ActivityId { get; set; }
        public int? YearId { get; set; }        
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string NameGroup { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }        
        public DateTime DateEnrollment { get; set; }
        public DateTime? DateExclusion { get; set; }
        public string Cause { get; set; }
        public bool Transfer { get; set; }
        public string Note { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string ParentPhone { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string School { get; set; }
        public int? Class { get; set; }
        public string Teacher { get; set; }
    }
}