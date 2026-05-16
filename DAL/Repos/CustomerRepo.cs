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

        public List<Payment> GetPaymentsByCustomer(int customerId) =>
            db.Payments.Where(p => p.Reservation.CustomerId == customerId).ToList();
    }
}