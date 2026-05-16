using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class ReceptionistDashboardDTO
    {
        public int TodayCheckIns { get; set; }
        public int TodayCheckOuts { get; set; }
        public decimal RevenueToday { get; set; }
        public int RoomsToClean { get; set; }

        public List<RoomAvailabilityDTO> CleaningAlerts { get; set; } = new();
        public List<ReservationDTO> Reservations { get; set; } = new();
        public List<CustomersDTO> Customers { get; set; } = new();
        public List<RoomAvailabilityDTO> Rooms { get; set; } = new();
        public List<PaymentDTO> Payments { get; set; } = new();
    }

  

    public class ReservationRoomDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public decimal PricePerNight { get; set; }
    }

    public class ReservationSearchDTO
    {
        public string CustomerName { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
    }

    public class CustomersDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsVIP { get; set; }
    }

   

    public class RoomAvailabilityDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } // Available/Occupied/Maintenance/Cleaning
    }

    public class InvoiceDTO
    {
        public int ReservationId { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public int Nights { get; set; }
        public List<ReservationRoomDTO> Rooms { get; set; } = new();
    }
}