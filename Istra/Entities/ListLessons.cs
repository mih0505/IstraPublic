using System;

namespace Istra
{
    public class ListLessons
    {
        public int GroupId { get; set; }
        public int DirectionId { get; set; }
        public int TeacherId { get; set; }
        public int HousingId { get; set; }
        public string Teacher { get; set; }
        public DateTime DateLesson { get; set; }
        public int Number { get; set; }
        public string GroupName { get; set; }
        public int Students { get; set; }
        public byte DurationLesson { get; set; }
        public string CourseName { get; set; }
        public string DirectionName { get; set; }
        public string Branch { get; set; }
        public string Class { get; set; }            
        public string Topic { get; set; }
        public decimal Wage { get; set; }
        public decimal Wages { get { return (DurationLesson != null && Wage != null) ? DurationLesson * Wage : 0; } }
    }
}