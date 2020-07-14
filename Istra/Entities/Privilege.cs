using System.Collections.Generic;

namespace Istra.Entities
{
    public class Privilege
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public Privilege()
        {
            Enrollments = new List<Enrollment>();
        }
    }
}