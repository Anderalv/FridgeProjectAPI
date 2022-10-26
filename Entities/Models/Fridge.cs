using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Fridge
    {
        // [Column("IdFridge")]
        // public int Id { get; set; }
        //
        // public String Name { get; set; }
        // public String OwnerName { get; set; }
        //
        //
        // [ForeignKey(nameof(Model))]
        // public int IdModel { get; set; }
        // public Model Model { get; set; }
        
        [Column("IdFridge")]
        public int Id { get; set; }
        
        public String Name { get; set; }
        public String OwnerName { get; set; }
        
        
        [ForeignKey(nameof(Model))]
        public int IdModel { get; set; }
        
        
    }
}