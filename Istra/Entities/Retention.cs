using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Istra.Entities
{
    public class Retention
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public DateTime Date { get; set; }//дата внесения записи
        public DateTime Month { get; set; }//месяц операции
        public int? BaseId { get; set; }
        public Base Base { get; set; }
        [ForeignKey("TypeOfTransaction")]
        public int TypeId { get; set; }
        public TypeOfTransaction TypeOfTransaction { get; set; }// 1 - удержание, 2 - начисление, 3 - выплата
        public decimal Value { get; set; }

        //Доп. свойства 
        public string Name { get; set; }
        public int? Count { get; set; }
        public int? Hours { get; set; }
        public decimal? Wage { get; set; }
        
    }
}
