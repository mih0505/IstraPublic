using System.Collections.Generic;

namespace Istra.Entities
{
    public class Cause //причина отчисления
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public Cause()
        {
            Enrollments = new List<Enrollment>();
        }
    }
}