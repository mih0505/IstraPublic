using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public bool IsCredit { get; set; } // Зачет или оценка?
        public bool IsTypeGrade { get; set; } // Указывать ли оценку для раздела при печати
        public string Name { get; set; }
        public int Duration { get; set; }
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public int? LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
