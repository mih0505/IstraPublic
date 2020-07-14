namespace Istra
{
    public class ListTransactions
    {        
        public string Name { get; set; }
        public System.DateTime? Date { get; set; }
        public System.DateTime? Period { get; set; }
        public int? Count { get; set; }
        public int? Hours { get; set; }
        public decimal? Wage { get; set; }
        public decimal Total { get; set; }
    }
}