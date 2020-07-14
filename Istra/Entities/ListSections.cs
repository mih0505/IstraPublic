using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class ListSections
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public string ShortDate { get { return Date.ToShortDateString(); } }
        public bool IsCredit { get; set; } // Зачет или оценка?
        public bool IsTypeGrade { get; set; } // Указывать ли оценку для раздела при печати
        public string Credit { get { return (IsCredit) ? "Да" : "Нет"; } }
        public string TypeGrade { get { return (IsTypeGrade) ? "Да" : "Нет"; } }
        public int? CourseId { get; set; }
        public int? LessonId { get; set; }
        public int? GroupId { get; set; }
    }
}
