using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class TypeOfTransaction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        List<Retention> Retentions { get; set; }

        public TypeOfTransaction()
        {
            Retentions = new List<Retention>();
        }
    }
}
