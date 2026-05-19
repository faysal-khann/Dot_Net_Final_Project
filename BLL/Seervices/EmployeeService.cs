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

        public EmployeeDashboardDTO GetDashboardData(int userId)
        {
            // 1. Fetch real employee data from DB
            var emp = repo.GetEmployeeByUserId(userId);
            if (emp == null) return null; // Handle if User has no Employee profile

            var data = new EmployeeDashboardDTO
            {
                EmployeeId = emp.EmployeeId,
                EmployeeName = emp.Name,
                Position = emp.Position,
                ApprovalStatus = emp.ApprovalStatus
            };

            // 2. Universal Data (Schedules)
            var reservations = repo.GetTodaysReservations();
            var today = DateOnly.FromDateTime(DateTime.Today);

            data.TodaysCheckIns = reservations.Where(r => r.CheckInDate == today)
                .SelectMany(r => r.ReservationRooms.Select(rr => new ReservationSummaryDTO { RoomNumber = rr.Room.RoomNumber, GuestName = r.Customer.Name })).ToList();

            data.TodaysCheckOuts = reservations.Where(r => r.CheckOutDate == today)
                .SelectMany(r => r.ReservationRooms.Select(rr => new ReservationSummaryDTO { RoomNumber = rr.Room.RoomNumber, GuestName = r.Customer.Name })).ToList();

            data.TotalGuestsToday = data.TodaysCheckIns.Count + data.TodaysCheckOuts.Count; // Useful for Chef

            // 3. Role-Specific Data Loading
            if (emp.Position == "Cleaner")
            {
                var rooms = repo.GetRoomsByStatus("Cleaning");
                data.Tasks = rooms.Select(r => new RoomTaskDTO { RoomId = r.RoomId, RoomNumber = r.RoomNumber, Status = r.Status }).ToList();
            }
            else if (emp.Position == "Technician")
            {
                var rooms = repo.GetRoomsByStatus("Maintenance");
                data.Tasks = rooms.Select(r => new RoomTaskDTO { RoomId = r.RoomId, RoomNumber = r.RoomNumber, Status = r.Status }).ToList();
            }
            else if (emp.Position == "Manager")
            {
                data.PendingEmployees = repo.GetPendingEmployees().Select(e => new PendingEmployeeDTO { EmployeeId = e.EmployeeId, Name = e.Name, Position = e.Position }).ToList();
                var allRooms = repo.GetAllRooms();
                data.AvailableRooms = allRooms.Count(r => r.Status == "Available");
                data.OccupiedRooms = allRooms.Count(r => r.Status == "Occupied");
            }

            return data;
        }
        public EmployeeDTO GetEmployeeByUserId(int userId)
        {
            var c = repo.GetEmployeeByUserId(userId);

            return new EmployeeDTO
            {
                EmployeeId = c.EmployeeId,
                Name = c.Name,
                
            };
        }


        // Actions
        public bool MarkRoomAvailable(int roomId) => repo.UpdateRoomStatus(roomId, "Available");
        public bool ReportMaintenanceIssue(int roomId) => repo.UpdateRoomStatus(roomId, "Maintenance");
        public bool ApproveEmployee(int empId) => repo.ApproveEmployee(empId);
    }
}