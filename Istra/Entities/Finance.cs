namespace Istra
{
    public class Finance
    {
        public int StudentId { get; set; }
        public int GroupId { get; set; }
        public int? SchoolId { get; set; }
        public int EnrollId { get; set; }
        public int? PrivilegeId { get; set; }
        public int ActivityId { get; set; }
        public int YearId { get; set; }
        public string Activity { get; set; }
        public int StatusId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string NameGroup { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public string Privilege { get; set; }        
        public string Sex { get; set; }
        public string School { get; set; }
        public int? Class { get; set; }
        public double Accrual { get; set; }
        public double AccrualDiscount { get; set; }
        public double Payment { get; set; }

        public double Saldo { get { return AccrualDiscount - Payment; } }
    }
}