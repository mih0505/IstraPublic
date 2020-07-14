using System.Collections.Generic;

namespace Istra.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public int DurationCourse { get; set; }
        public int DurationExpress { get; set; }
        public string Note { get; set; }
        public int? DirectionId { get; set; }
        public Direction Direction { get; set; }
        public int? DocumentId { get; set; }
        public Document Document { get; set; }
        public bool IsRemoved { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Group> Groups { get; set; }
        public List<Section> Sections { get; set; }
        public Course()
        {
            Topics = new List<Topic>();
            Groups = new List<Group>();
            Sections = new List<Section>();
        }
    }
}