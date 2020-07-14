using System.Collections.Generic;

namespace Istra.Entities
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsRemoved { get; set; }
        public List<Student> Students { get; set; }
        public School()
        {
            Students = new List<Student>();
        }
    }
}