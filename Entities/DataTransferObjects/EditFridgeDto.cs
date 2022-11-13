using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class EditFridgeDto
    {
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the Name is 2 characters.")]
        public String Name { get; set; }
        [Required(ErrorMessage = "Name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the Name is 2 characters.")]
        public String OwnerName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "FridgeId is required and it can't be lower than 1")]
        public int IdModel { get; set; }
        
        
        public List<int> AddedProducts{ get; set; }
        public ICollection<ProductForEditInFridgeDto> EditedProducts { get; set; }
    }
}