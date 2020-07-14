using System.Collections.Generic;

namespace Istra.Entities
{
    public class Month
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsRemoved { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Payment> Payments { get; set; }
        public Month()
        {
            Enrollments = new List<Enrollment>();
            Payments = new List<Payment>();
        }
    }
}