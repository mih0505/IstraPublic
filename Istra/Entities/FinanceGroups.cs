namespace Istra
{
    public class FinanceGroups
    {
        public int GroupId { get; set; }
        public int DirectionId { get; set; }
        public string Direction { get; set; }
        public int ActivityId { get; set; }
        public string CourseName { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public int? StudentsP { get; set; }
        public int StudentsF { get; set; }
        public string Activity { get; set; }
        public double? Price { get; set; }
        public double PlanAccrual { get { return (StudentsP != null && Price != null) ? (double)StudentsP * (double)Price : 0; } }
        public double Accrual { get; set; }
        public double AccrualDiscount { get; set; }
        public double Payment { get; set; }
        public double Saldo { get { return AccrualDiscount - Payment; } }

        public int YearId { get; set; }
    }
}