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

        // Get Rooms that need cleaning or maintenance
        public List<Room> GetPendingRoomTasks()
        {
            return db.Rooms
                     .Where(r => r.Status == "Cleaning" || r.Status == "Maintenance")
                     .ToList();
        }

        // Update Room Status (Cleaning -> Available, or -> Maintenance)
        public bool UpdateRoomStatus(int roomId, string newStatus)
        {
            var room = db.Rooms.Find(roomId);
            if (room != null)
            {
                room.Status = newStatus;

                // Extra: If marking as Available, you could log this in an Audit table here
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