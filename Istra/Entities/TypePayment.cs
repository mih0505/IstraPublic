using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class TypePayment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortname { get; set; }
        public List<Payment> Payments { get; set; }
        public TypePayment()
        {
            Payments = new List<Payment>();
        }
    }
}
