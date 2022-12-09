using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Product
{
    public abstract class ProductForCreateUpdate
    {
        
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the Name is 3 characters.")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity is required and it can't be lower than 1")]
        public int Quantity { get; set; }
    }
}