using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class RoomTypeDTO
    {
        public int RoomTypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public int Capacity { get; set; } 
    }
}
