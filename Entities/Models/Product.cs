using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Product
    {
        [Column("IdProduct")]
        public int Id { get; set; }
        public String Name { get; set; }
        public int DefaultQuantity { get; set; }
        
        public ICollection<Fridge> Fridges { get; set; }
        public ICollection<FridgeProduct> FridgeProducts { get; set; }
    }
}