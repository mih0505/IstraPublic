namespace Istra.Entities
{
    public class Study
    {
        public long Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public string Grade { get; set; }
    }
}