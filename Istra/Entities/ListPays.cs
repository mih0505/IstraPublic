using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class ListPays
    {
        public bool IsSelected { get; set; }
        public int PaymentId { get; set; }
        public int EnrollmentId { get; set; }
        public int WorkerId { get; set; }
        public int GroupId { get; set; }
        public int StudentId { get; set; }
        public DateTime DatePayment { get; set; }
        public double ValuePayment { get; set; }
        public string GroupName { get; set; }
        public string StudentLastname { get; set; }
        public string StudentFirstname { get; set; }
        public string StudentMiddlename { get; set; }
        public string WorkerLastnameFM { get; set; }
        public string Note { get; set; }
        public string Housing { get; set; }
        public string Type { get; set; }
    }
}
