using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = null!;
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = null!;
        public int Capacity { get; set; } 
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
    }
}
