using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class Direction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public List<Course> Courses { get; set; }
        public Direction()
        {
            Courses = new List<Course>();
        }
    }
}
