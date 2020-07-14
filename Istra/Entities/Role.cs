using System.Collections.Generic;

namespace Istra.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public List<Worker> Workers { get; set; }
        public int Priority { get; set; }
        public Role()
        {
            Workers = new List<Worker>();
        }
    }
}