using System;

namespace Entities.DataTransferObjects
{
    public class FridgeDto
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String OwnerName { get; set; }
        public int IdModel { get; set; }
    }
}