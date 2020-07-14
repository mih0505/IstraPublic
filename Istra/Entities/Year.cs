using System.Collections.Generic;

namespace Istra.Entities
{
    public class Year
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRemoved { get; set; }
        public int SortIndex { get; set; }
        public List<Group> Groups { get; set; }
        public List<Template> Templates { get; set; }
        public Year()
        {
            Templates = new List<Template>();
            Groups = new List<Group>();
        }
    }
}