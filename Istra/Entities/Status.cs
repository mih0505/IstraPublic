using System.Collections.Generic;

namespace Istra.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public List<Student> Students { get; set; }
        public List<School> Schools { get; set; }
        public Status()
        {
            Students = new List<Student>();
            Schools = new List<School>();
        }
    }
}