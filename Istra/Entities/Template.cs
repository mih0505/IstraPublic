using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? YearId { get; set; }
        public Year Year { get; set; }
        public List<TemplateRate> Rates { get; set; }
        public List<TemplateGroup> Groups { get; set; }
        public Template()
        {
            Rates = new List<TemplateRate>();
            Groups = new List<TemplateGroup>();
        }
    }
}
