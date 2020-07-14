using System.Collections.Generic;

namespace Istra.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public int? SortIndex { get; set; }

        List<Worker> Workers { get; set; }
        public Department()
        {
            Workers = new List<Worker>();
        }
    }
}