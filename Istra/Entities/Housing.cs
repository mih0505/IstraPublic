using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class Housing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public bool IsRemoved { get; set; }

        public List<Class> Classes { get; set; }
        public List<Payment> Payments { get; set; }
        public Housing()
        {
            Classes = new List<Class>();
            Payments = new List<Payment>();
        }
    }
}
