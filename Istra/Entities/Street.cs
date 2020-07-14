using System.Collections.Generic;

namespace Istra.Entities
{
    public class Street
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public bool IsRemoved { get; set; }
        public List<Student> Students { get; set; }
        public Street()
        {
            Students = new List<Student>();
        }
    }
}