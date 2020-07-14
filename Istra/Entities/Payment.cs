using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Istra.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }        
        public DateTime DatePayment { get; set; }
        public int? MonthId { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public double ValuePayment { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public int TypePaymentId { get; set; }
        public TypePayment TypePayment { get; set; }
        public string Note { get; set; }
        public int? HousingId { get; set; }
        public Housing Housing { get; set; }

        public bool AdditionalPay { get; set; }
        
        public bool IsDeleted { get; set; }
        public int? RemovedWorkerId { get; set; }
        [InverseProperty("RemovedWorker")]
        [ForeignKey("RemovedWorkerId")]
        public Worker Worker1 { get; set; }


    }
}
