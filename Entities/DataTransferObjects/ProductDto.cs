using System;

namespace Entities.DataTransferObjects
{
    public class ProductDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}