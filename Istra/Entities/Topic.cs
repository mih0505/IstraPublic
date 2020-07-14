using System.Collections.Generic;

namespace Istra.Entities
{
    public class Topic//темы
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public List<Lesson> Lessons { get; set; }
        public Topic()
        {
            Lessons = new List<Lesson>();
        }
    }
}