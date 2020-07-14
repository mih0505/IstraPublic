namespace Istra
{
    internal class ListWorkersAndBalance
    {
        public int WorkerId { get; set; }
        public decimal Profit { get; set; }
        public decimal Retention { get; set; }
        public decimal Payment { get; set; }
        public decimal Balance { get { return Profit - Retention - Payment; } }
        public int CurrentMonthTransactions { get; set; }
    }
}