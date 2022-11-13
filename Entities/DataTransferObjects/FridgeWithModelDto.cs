using System;

namespace Entities.DataTransferObjects
{
    public class FridgeWithModelDto
    {
        public int IdFridge { get; set; }
        public String Name { get; set; }
        public String OwnerName { get; set; }
        public int IdModel { get; set; }
        public String ModelName { get; set; }
        public int Year { get; set; }
       
       
        
    }
}