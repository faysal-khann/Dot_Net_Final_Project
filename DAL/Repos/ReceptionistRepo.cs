using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class ReceptionistRepo
    {
        private readonly HotelManagementContext db;
        public ReceptionistRepo(HotelManagementContext db) { this.db = db; }

        // Reservations
        public List<Reservation> GetAllReservations() =>
            db.Reservations.Include(r => r.Customer)
                           .Include(r => r.ReservationRooms).ThenInclude(rr => rr.Room)
                           .ToList();

        public Reservation GetReservation(int id) =>
            db.Reservations.Include(r => r.Customer)
                           .Include(r => r.ReservationRooms).ThenInclude(rr => rr.Room)
                           .FirstOrDefault(r => r.ReservationId == id);

        public bool AddReservation(Reservation r) { db.Reservations.Add(r); return db.SaveChanges() > 0; }
        public bool UpdateReservation(Reservation r) { db.Reservations.Update(r); return db.SaveChanges() > 0; }

        // Customers
        public List<Customer> GetCustomers() => db.Customers.ToList();
        public Customer GetCustomer(int id) => db.Customers.Find(id);
        public bool AddCustomer(Customer c) { db.Customers.Add(c); return db.SaveChanges() > 0; }
        public bool UpdateCustomer(Customer c) { db.Customers.Update(c); return db.SaveChanges() > 0; }

        // Rooms
        public List<Room> GetRooms() => db.Rooms.Include(r => r.RoomType).ToList();

        // Payments
        public bool AddPayment(Payment p) { db.Payments.Add(p); return db.SaveChanges() > 0; }
        public List<Payment> GetPayments() => db.Payments.ToList();
        
    }
}