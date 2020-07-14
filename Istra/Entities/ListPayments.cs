using System;

namespace Istra
{
    public class ListPayments
    {
        public int Id { get; set; }
        public DateTime DatePayment { get; set; }
        public string Type { get; set; }
        public double ValuePayment { get; set; }
        public string Month { get; set; }
        public string Worker { get; set; }
        public string Note { get; set; }
        public bool AdditionalPay { get; set; }
    }
}