using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Istra.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string LastnameEn { get; set; }
        public string FirstnameEn { get; set; }
        public DateTime DateOfBirth { get; set; }
        [StringLength(1)]
        public string Sex { get; set; }
        public string LastnameParent { get; set; }
        public string FirstnameParent { get; set; }
        public string MiddlenameParent { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssuedBy { get; set; }
        public DateTime? PassportDate { get; set; }
        public DateTime EntryDate { get; set; }//дата записи
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int? SchoolId { get; set; }
        public School School { get; set; }
        public int? Class { get; set; }
        public string Shift { get; set; }
        public string StudentPhone { get; set; }
        public string StudentPhone2 { get; set; }
        public string ParentsPhone { get; set; }
        public DateTime DateAddBase { get; set; }
        public int? WorkerId { get; set; }
        public Worker Worker { get; set; }
        public string Note { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public int? StreetId { get; set; }
        public Street Street { get; set; }
        public string House { get; set; }
        public int? Float { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Study> Studies { get; set; }

        public string LastnameFM
        {
            get
            {
                string lastnamefm = "";
                if (Lastname != null)
                    lastnamefm = Lastname;
                if (Firstname != null)
                    lastnamefm += " "+Firstname.Substring(0, 1)+".";
                if (Middlename != null)
                    lastnamefm += Middlename.Substring(0, 1) + ".";

                return lastnamefm;
            }
        }

        public Student()
        {
            Enrollments = new List<Enrollment>();
            Studies = new List<Study>();
        }

        public string Fullname()
        {
            return Lastname + " " + Firstname + " " + Middlename;
        }
        public string FullnameParent()
        {
            return LastnameParent + " " + FirstnameParent + " " + MiddlenameParent;
        }
        public string GetPassport()
        {
            return "№ " + PassportNumber + " выдан " + PassportIssuedBy + " от " + Convert.ToDateTime(PassportDate).ToShortDateString();
        }
        public string GetPassportNumber()
        {
            return "№ " + PassportNumber + " выдан";
        }
        public string GetPassportIssuedBy()
        {
            return PassportIssuedBy + " от " + Convert.ToDateTime(PassportDate).ToShortDateString();
        }
    }
}
