using System.Collections.Generic;

namespace Istra.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }

        public List<Group> Groups { get; set; }
        public Activity()
        {
            Groups = new List<Group>();
        }
    }
}