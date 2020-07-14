using System;
using System.Collections.Generic;

namespace Istra.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public int? CourseId { get; set; }
        public Course Course { get; set; }
        public int? YearId { get; set; }
        public Year Year { get; set; }        
        public int? ClassId { get; set; }
        public Class Class { get; set; }
        public DateTime Begin { get; set; }
        public string Days { get; set; }
        public byte DurationLesson { get; set; }//продолжительность занятия в часах
        public int DurationCourse { get; set; }//продолжительность курса в часах
        public int? TeacherId { get; set; }
        public virtual Worker Teacher { get; set; }
        public bool TwoTeachers { get; set; }
        public double Value { get; set; }
        public string NumberProtocol { get; set; }
        public string NumberDocument { get; set; }
        public int? TotalPoint { get; set; }
        public string Note { get; set; }
        public int? UnvisibleLessons { get; set; }
        public int? DocumentId { get; set; }
        public Document Document { get; set; }
        public int? GroupCreatorId { get; set; }
        public virtual Worker GroupCreator { get; set; }
        public bool Individual { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Study> Stadies { get; set; }
        public List<Section> Sections { get; set; }
        public List<TemplateGroup> TemplateGroups { get; set; }
        public int? PlanEnroll { get; set; }

        public Group()
        {
            Enrollments = new List<Enrollment>();
            Lessons = new List<Lesson>();
            Schedules = new List<Schedule>();
            Payments = new List<Payment>();
            Stadies = new List<Study>();
            Sections = new List<Section>();
            TemplateGroups = new List<TemplateGroup>();
        }

        public string EndTimeLesson
        {
            get { return (Begin.AddMinutes(DurationLesson * 45)).ToShortTimeString(); }
        }
    }
}
