using System;

namespace Istra
{
    public class ListGroupFull
    {
        public int Id { get; set; }
        public string Direction { get; set; }
        public string Course { get; set; }
        public string Group { get; set; }
        public string StatusGroup { get; set; }
        public string Branch { get; set; }
        public string Class { get; set; }
        public string Days { get; set; }
        public DateTime Begin { get; set; }
        public byte DurationLesson { get; set; }
        public int DurationCourse { get; set; }
        public int CountLesson { get; set; }
        public int PassedLesson { get; set; }
        public string Teacher { get; set; }
        public bool TwoTeachers { get; set; }
        public int Students { get; set; }
        public string Year { get; set; }
        public double SchedSumNow { get; set; }
        public double PaysSumNow { get; set; }
        public string SaldoNow { get; set; }
    }
}