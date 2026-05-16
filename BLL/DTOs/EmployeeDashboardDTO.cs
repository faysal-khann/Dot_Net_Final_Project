using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class EmployeeDashboardDTO
    {
        public string EmployeeName { get; set; }
        public string ApprovalStatus { get; set; } // To show "Pending" or "Approved"

        // Task List (Ordered by CheckOut date ideally)
        public List<RoomTaskDTO> Tasks { get; set; } = new List<RoomTaskDTO>();

        // Read-only views
        public List<ReservationSummaryDTO> TodaysCheckIns { get; set; } = new List<ReservationSummaryDTO>();
        public List<ReservationSummaryDTO> TodaysCheckOuts { get; set; } = new List<ReservationSummaryDTO>();

        // Work History
        public int RoomsServicedThisMonth { get; set; }
    }

    public class RoomTaskDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string Status { get; set; } // "Cleaning", "Maintenance"
    }

    public class ReservationSummaryDTO
    {
        public string RoomNumber { get; set; }
        public string GuestName { get; set; }
    }
}