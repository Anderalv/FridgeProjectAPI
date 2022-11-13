using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class ProductForEditInFridgeDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "FridgeId is required and it can't be lower than 1")]
        public int IdProduct { get; set; }
        
        [Range(0, 30, ErrorMessage = "Quantity is required and it can't be lower than 0")]
        public int EditedQuantity { get; set; }
    }
}