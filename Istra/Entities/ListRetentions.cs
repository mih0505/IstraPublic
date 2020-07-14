using System;

namespace Istra
{
    public class ListRetentions
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Month { get; set; }
        public string Teacher { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
    }
}