using System;
using System.Collections.Generic;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class ProductForUpdateDto
    {
        public String Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}