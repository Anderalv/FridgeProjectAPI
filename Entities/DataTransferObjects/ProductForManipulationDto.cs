using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class ProductForManipulationDto
    {
        
        [Range(1, int.MaxValue, ErrorMessage = "FridgeId is required and it can't be lower than 1")]
        public int FridgeId { get; set; }
    
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the Name is 2 characters.")]
        public string Name { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Quantity is required and it can't be lower than 1")]
        public int Quantity { get; set; }
    }
}