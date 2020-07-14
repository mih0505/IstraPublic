using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public List<Course> Courses { get; set; }
        public List<Group> Groups { get; set; }
        public Document()
        {
            Courses = new List<Course>();
            Groups = new List<Group>();            
        }
    }
}
