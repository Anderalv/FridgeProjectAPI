using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class FridgeProduct
    {
        [Column("IdFridgeProduct")]
        public int Id { get; set; } 
        
        [ForeignKey(nameof(Product))]
        public int IdProduct { get; set; }
        public Product Product { get; set; }
        
        [ForeignKey(nameof(Fridge))]
        public int IdFridge { get; set; }
        public Fridge Fridge { get; set; }
        public int Quantity { get; set; }
    }
}