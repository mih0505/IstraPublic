using System;

namespace Istra
{
    public class ListStudentFull
    {
        public int EnrollmentId { get; set; }
        public int YearId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string StatusStudent { get; set; }
        public string School { get; set; }
        public int? Class { get; set; }
        public string Shift { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string LastnameParent { get; set; }
        public string FirstnameParent { get; set; }
        public string MiddlenameParent { get; set; }
        public string PhoneNumberParent { get; set; }
        public string Group { get; set; }
        public string Year { get; set; }
        public string StatusGroup { get; set; }
        public double SchedSumNow { get; set; }
        public double PaysSumNow { get; set; }
        public string SaldoNow { get; set; }
    }
}