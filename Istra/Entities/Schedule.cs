using System;

namespace Istra.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? CountPayments { get; set; }
        public int Source { get; set; }//1 - источник расчетов для графика платежей, 2 - сам график платежей, 3 - источник расчетов скидки
        public double Value { get; set; }
        public double Discount { get; set; }
        public int? EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }        
        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }
        public string Note { get; set; }

        public Schedule()
        {
            Discount = 0;
        }
    }
}
