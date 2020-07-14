using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class PrintContract
    {
        public Enrollment Enroll { get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        public Year Year { get; set; }
        public Course Course { get; set; }
        public Direction Direction { get; set; }
        public Document Document { get; set; }
        public City City { get; set; }
        public List<ListSchedules> Schedules { get; set; }        
    }
}
