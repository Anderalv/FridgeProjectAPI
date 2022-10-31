using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class ProductForManipulationDto
    {
    [Range(1, int.MaxValue, ErrorMessage = "FridgeId is required and it can't be lower than 1")]
    public int FridgeId { get; set; }
        
    [Required(ErrorMessage = "Name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string Name { get; set; }
        
    [Range(1, int.MaxValue, ErrorMessage = "Quantity is required and it can't be lower than 1")]
    public int Quantity { get; set; }
    }
}