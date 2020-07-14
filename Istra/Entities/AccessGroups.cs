using Istra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class AccessGroups
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
