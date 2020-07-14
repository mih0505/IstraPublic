using System.Collections.Generic;

namespace Istra.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool defaultCity { get; set; }
        public List<Student> Students { get; set; }
        public List<Street> Streets { get; set; }
        public bool IsRemoved { get; set; }
        public City()
        {
            Students = new List<Student>();
            Streets = new List<Street>();
        }
    }
}