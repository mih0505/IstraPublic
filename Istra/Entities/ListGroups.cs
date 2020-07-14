using System;

namespace Istra
{
    public class ListGroups
    {
        public int GroupId { get; set; }
        public int DirectionId { get; set; }
        public int ActivityId { get; set; }
        public string Activity { get; set; }
        public string Direction { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Branch { get; set; }
        public string Class { get; set; }
        public byte DurationLesson { get; set; }
        public string Days { get; set; }        
        public DateTime Begin { get; set; }
        public int? YearId { get; set; }        
        public bool? Type { get; set; }
        public string Teacher { get; set; }
        public bool TwoTeachers { get; set; }
        public int Students { get; set; }
        public string Note { get; set; }
        public int TeacherId { get; set; }

    }
}