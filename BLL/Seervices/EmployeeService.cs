using BLL.DTOs;
using DAL.Repos;
using System;
using System.Linq;

namespace BLL.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepo repo;

        public EmployeeService(EmployeeRepo repo)
        {
            this.repo = repo;
        }

        public EmployeeDashboardDTO GetDashboardData(int employeeId, string employeeName, string approvalStatus)
        {
            var data = new EmployeeDashboardDTO
            {
                EmployeeName = employeeName,
                ApprovalStatus = approvalStatus
            };

            // 1. Get Tasks (Rooms needing cleaning)
            var rooms = repo.GetPendingRoomTasks();
            data.Tasks = rooms.Select(r => new RoomTaskDTO
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                Status = r.Status
            }).ToList();

            // 2. Get Today's Check-ins and Check-outs
   
            var reservations = repo.GetTodaysReservations();
            var today = DateOnly.FromDateTime(DateTime.Today);
            // Map Check-ins
            data.TodaysCheckIns = reservations

                .Where(r => r.CheckInDate == today)
                .SelectMany(r => r.ReservationRooms.Select(rr => new ReservationSummaryDTO
                {
                    RoomNumber = rr.Room.RoomNumber,
                    GuestName = r.Customer.Name
                })).ToList();

            // Map Check-outs
            data.TodaysCheckOuts = reservations
                .Where(r => r.CheckOutDate == today)
                .SelectMany(r => r.ReservationRooms.Select(rr => new ReservationSummaryDTO
                {
                    RoomNumber = rr.Room.RoomNumber,
                    GuestName = r.Customer.Name
                })).ToList();

            // Mocking work history for now
            data.RoomsServicedThisMonth = 42;

            return data;
        }

        public bool MarkRoomAvailable(int roomId)
        {
            return repo.UpdateRoomStatus(roomId, "Available");
        }

        public bool ReportMaintenanceIssue(int roomId)
        {
            return repo.UpdateRoomStatus(roomId, "Maintenance");
        }
    }
}