using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class TemplateRate
    {
        public int Id { get; set; }
        public int CountStudents { get; set; }
        public decimal Wage { get; set; }
        public int TemplateId { get; set; }
        public Template Template { get; set; }
    }
}
