using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class CustomerRepo
    {
        private readonly HotelManagementContext db;
        public CustomerRepo(HotelManagementContext db) { this.db = db; }

        public Customer GetCustomer(int id) => db.Customers.Find(id);

        public List<Room> GetRooms() =>
            db.Rooms.Include(r => r.RoomType).ToList();
        public List<RoomType> getRoomType()
        {
            return db.RoomTypes.ToList();
        }
        public List<Reservation> GetReservationsByCustomer(int customerId) =>
            db.Reservations.Include(r => r.ReservationRooms).ThenInclude(rr => rr.Room)
                           .Where(r => r.CustomerId == customerId).ToList();

        public bool AddReservation(Reservation r)
        {
            db.Reservations.Add(r);
            return db.SaveChanges() > 0;
        }

        public bool AddPayment(Payment p)
        {
            db.Payments.Add(p);
            return db.SaveChanges() > 0;
        }
        public bool AddReservationRoom(ReservationRoom r)
        {
            db.ReservationRooms.Add(r);
            return db.SaveChanges() > 0;

        }
        public List<Payment> GetPaymentsByCustomer(int customerId) =>
            db.Payments.Where(p => p.Reservation.CustomerId == customerId).ToList();

        public Customer GetCustomerByUserId(int userId)
        {
            return db.Customers
                     .FirstOrDefault(c => c.UserId == userId);
        }

        public ReservationRoom GetRoomByReservationID(int reservationId)
        {
                        return db.ReservationRooms.FirstOrDefault(rr => rr.ReservationId == reservationId);
        }
        public List<Room> GetAvailableRooms(DateTime reqCheckIn, DateTime reqCheckOut, string type, decimal? minPrice, decimal? maxPrice)
        {
             var checkIn = DateOnly.FromDateTime(reqCheckIn);
    var checkOut = DateOnly.FromDateTime(reqCheckOut);
            // Step A: Find IDs of rooms that are already booked during these dates
            // (Formula: ExistingCheckIn < RequestedCheckOut AND ExistingCheckOut > RequestedCheckIn)
            var bookedRoomIds = db.Reservations
                .Where(res => res.Status != "Cancelled" &&
                              res.CheckInDate < checkOut         &&
                              res.CheckOutDate > checkIn)
                .Select(res => res.RoomId)
                .Distinct()
                .ToList();

            // Step B: Get all rooms EXCEPT the ones we just found
            var query = db.Rooms.Include(r => r.RoomType)
                          .Where(r => !bookedRoomIds.Contains(r.RoomId) && r.Status != "Maintenance");

            // Apply optional filters
            if (!string.IsNullOrEmpty(type))
                query = query.Where(r => r.RoomType.TypeName.Contains(type));
            if (minPrice.HasValue)
                query = query.Where(r => r.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice.Value);

            return query.ToList();
        }
        public bool CancelReservation(int reservationId)
        {
            var reservation = db.Reservations.Find(reservationId);

            if (reservation != null)
            {
                // 1. Change reservation status to Cancelled
                reservation.Status = "Cancelled";

                // 2. Find the payment and remove it (Refund)
                var payment = db.Payments.FirstOrDefault(p => p.ReservationId == reservationId);
                if (payment != null)
                {
                    db.Payments.Remove(payment);
                }

                // 3. Save both changes to the database
                return db.SaveChanges() > 0;
            }

            return false;
        }
    }
}