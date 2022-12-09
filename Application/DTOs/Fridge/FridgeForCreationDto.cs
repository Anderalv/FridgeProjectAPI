using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Fridge
{
    public class FridgeForCreationDto
    {
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the Name is 3 characters.")]
        public String Name { get; set; }
        
        [MaxLength(30, ErrorMessage = "Maximum length for the OwnerName is 30 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the OwnerName is 3 characters.")]
        public String OwnerName { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "IdModel is required and it can't be lower than 1")]
        public int IdModel { get; set; }
        public List<int> ProductsId { get; set; }
    }
}