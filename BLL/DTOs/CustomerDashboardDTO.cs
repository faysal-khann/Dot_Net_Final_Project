using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class CustomerDashboardDTO
    {
        public CustomerpDTO Profile { get; set; }
        public List<ReservationDTO> MyReservations { get; set; } = new();
        public List<PaymentDTO> MyPayments { get; set; } = new();
        public List<RoomAvailabilityDTO> AvailableRooms { get; set; }
        public List<RoomTypeDTO> RoomTypes { get; set; } 

    }

    

    

   
}