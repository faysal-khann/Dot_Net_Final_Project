using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class EmployeeDashboardDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; } // Cleaner, Technician, Manager, Chef
        public string ApprovalStatus { get; set; }

        // Housekeeping & Maintenance Tasks
        public List<RoomTaskDTO> Tasks { get; set; } = new List<RoomTaskDTO>();
        public int RoomsServicedThisMonth { get; set; }

        // Read-only views & Chef info
        public List<ReservationSummaryDTO> TodaysCheckIns { get; set; } = new List<ReservationSummaryDTO>();
        public List<ReservationSummaryDTO> TodaysCheckOuts { get; set; } = new List<ReservationSummaryDTO>();
        public int TotalGuestsToday { get; set; } // Useful for the Chef to plan meals

        // Manager Info
        public List<PendingEmployeeDTO> PendingEmployees { get; set; } = new List<PendingEmployeeDTO>();
        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }
    }

    public class RoomTaskDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Status { get; set; }
    }

    public class ReservationSummaryDTO
    {
        public string RoomNumber { get; set; }
        public string GuestName { get; set; }
    }

    
}