using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Istra.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public DateTime DateEnrollment { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int? EnrollId { get; set; }  //id записавшего работника      
        [InverseProperty("Enrollments")]
        [ForeignKey("EnrollId")]
        public virtual Worker WorkerEnrollment { get; set; }
        public DateTime? DateExclusion { get; set; }
        public int? MonthExclusionId { get; set; }//месяц отчисления
        public Month MonthExclusion { get; set; }
        public string NumberDocument { get; set; }
        public int? CauseId { get; set; }//причина отчисления
        public Cause Cause { get; set; }
        public int? ExclusionId { get; set; }//id отчислившего работника
        [InverseProperty("Exclusions")]
        [ForeignKey("ExclusionId")]
        public Worker WorkerExclusion { get; set; }
        public int? PrivilegeId { get; set; }
        public Privilege Privilege { get; set; }
        public bool Transfer { get; set; }//зачислен переводом из другой группы
        public string Note { get; set; }

        public string AdditionalPays { get; set; }//содержит список доп платежей, не учтенных в основных

        public List<Payment> Payments { get; set; }
        public List<Schedule> Schedules { get; set; }
        public Enrollment()
        {
            Payments = new List<Payment>();
            Schedules = new List<Schedule>();
        }
    }
}
