using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Model
    {
        [Column("IdModel")]
        public int Id { get; set; }
        public String Name { get; set; }
        public int Year { get; set; }
        private ICollection<Fridge> Fridges { get; set; }
    }
}