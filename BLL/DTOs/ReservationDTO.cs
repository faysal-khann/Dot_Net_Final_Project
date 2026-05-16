using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ReservationRoomDTO> Rooms { get; set; } = new();
    }
}
