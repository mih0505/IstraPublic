using System;
using System.Collections.Generic;

namespace Istra.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? TeacherId { get; set; }
        public Worker Worker { get; set; }
        public int? ClassId { get; set; }
        public Class Class { get; set; }
        public int Number { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public byte DurationLesson { get; set; }
        public int? TopicId { get; set; }
        public Topic Topic { get; set; }
        public List<Study> Studies { get; set; }
        public List<Section> Sections { get; set; }

        public Lesson()
        {
            Studies = new List<Study>();
            Sections = new List<Section>();
        }
    }
}
