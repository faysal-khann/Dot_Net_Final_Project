using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class DashboardDTO
    {
        // Revenue & Occupancy Analytics
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public double OccupancyRate { get; set; } // e.g., 75.5 for 75.5%

        // Reservation Summary
        public int ConfirmedReservations { get; set; }
        public int PendingReservations { get; set; }
        public int CancelledReservations { get; set; }

        // Alerts
        public int PendingEmployeeApprovals { get; set; }
    }
}
