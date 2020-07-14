using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class ListPeriods
    {
        public int Id { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public double Value { get; set; }
        public int? CountPay { get; set; }
    }
}
