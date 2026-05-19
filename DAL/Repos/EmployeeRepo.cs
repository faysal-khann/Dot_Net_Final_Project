using DAL.EF;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class EmployeeRepo
    {
        private readonly HotelManagementContext db;

        public EmployeeRepo(HotelManagementContext db)
        {
            this.db = db;
        }
        

        // Fetch real employee profile
        public Employee GetEmployeeByUserId(int userId)
        {
            return db.Employees.FirstOrDefault(e => e.UserId == userId);
        }

        // General Task Fetcher (Pass "Cleaning" for Cleaners, "Maintenance" for Technicians)
        public List<Room> GetRoomsByStatus(string status)
        {
            return db.Rooms.Where(r => r.Status == status).ToList();
        }

        public List<Room> GetAllRooms() => db.Rooms.ToList();

        public bool UpdateRoomStatus(int roomId, string newStatus)
        {
            var room = db.Rooms.Find(roomId);
            if (room != null)
            {
                room.Status = newStatus;
                return db.SaveChanges() > 0;
            }
            return false;
        }

        // For Manager Dashboard
        public List<Employee> GetPendingEmployees()
        {
            return db.Employees.Where(e => e.ApprovalStatus == "Pending").ToList();
        }

        public bool ApproveEmployee(int employeeId)
        {
            var emp = db.Employees.Find(employeeId);
            if (emp != null)
            {
                emp.ApprovalStatus = "Approved";
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public List<Reservation> GetTodaysReservations()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return db.Reservations
                     .Include(r => r.Customer)
                     .Include(r => r.ReservationRooms)
                     .ThenInclude(rr => rr.Room)
                     .Where(r => r.CheckInDate == today || r.CheckOutDate == today)
                     .ToList();
        }
    }
}