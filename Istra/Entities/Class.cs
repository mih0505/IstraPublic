using System.Collections.Generic;

namespace Istra.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? HousingId { get; set; }
        public Housing Housing { get; set; }
        public bool IsRemoved { get; set; }
        public List<Group> Groups { get; set; }
        public List<Lesson> Classes { get; set; }
        public Class()
        {
            Classes = new List<Lesson>();
            Groups = new List<Group>();
        }
    }
}