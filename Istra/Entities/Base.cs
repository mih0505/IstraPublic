using System.Collections;
using System.Collections.Generic;

namespace Istra.Entities
{
    public class Base
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public List<Retention> Retentions { get; set; }

        public Base()
        {
            Retentions = new List<Retention>();
        }
    }
}