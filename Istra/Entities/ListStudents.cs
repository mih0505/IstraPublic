using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class ListStudent
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public int ActivityId { get; set; }
        public string Activity { get; set; }
        public string GroupActive { get; set; }
        public int? SchoolId { get; set; }
        public int EnrollId { get; set; }
        public int? PrivilegeId { get; set; }
        public int StatusId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public double Saldo { get; set; }
        public string NameGroup { get; set; }
        public string AdditionalPays { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string ParentPhone { get; set; }
        public DateTime DateEnrollment { get; set; }  
        public string Note { get; set; }
        public int YearId { get; set; }
        public string Year { get; set; }
        public DateTime Begin { get; set; }             
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string School { get; set; }
        public int? Class { get; set; }
        public string Shift { get; set; }        
        public string Status { get; set; }
        

        
    }
}
