using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Fridge
    {
        [Column("IdFridge")]
        public int Id { get; set; }
        public String Name { get; set; }
        public String OwnerName { get; set; }

        [ForeignKey(nameof(Model))]
        public int IdModel { get; set; }
        public Model Model { get; set; }
        
        public ICollection<Product> Products { get; set; }
        public ICollection<FridgeProduct> FridgeProducts { get; set; }


    }
}