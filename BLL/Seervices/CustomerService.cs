using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class CustomerService
    {
        private readonly CustomerRepo repo;
        public CustomerService(CustomerRepo repo) { this.repo = repo; }

        // DASHBOARD
        public CustomerDashboardDTO GetDashboard(int customerId)
        {
            var cust = repo.GetCustomer(customerId);
            var reservations = repo.GetReservationsByCustomer(customerId);
            var payments = repo.GetPaymentsByCustomer(customerId);

            var roomtype = repo.getRoomType();

            return new CustomerDashboardDTO
            {
                Profile = new CustomerpDTO
                {
                    CustomerId = cust.CustomerId,
                    Name = cust.Name,
                    Email = cust.Email,
                    Phone = cust.Phone,
                    IsVIP = reservations.Count >= 3
                },

                MyReservations = reservations.Select(r => new ReservationDTO
                {
                    ReservationId = r.ReservationId,
                    CustomerId = r.CustomerId,
                    CustomerName = cust.Name,
                    CheckInDate = r.CheckInDate.ToDateTime(TimeOnly.MinValue),
                    CheckOutDate = r.CheckOutDate.ToDateTime(TimeOnly.MinValue),
                    Status = r.Status,
                    TotalAmount = r.TotalAmount
                }).ToList(),
                RoomTypes = roomtype.Select(t => new RoomTypeDTO
                {
                    RoomTypeId = t.RoomTypeId,
                    TypeName = t.TypeName,
                    Capacity = t.Capacity
                }).ToList(),

                MyPayments = payments.Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    ReservationId = p.ReservationId,
                    Amount = p.Amount,
                    Method = p.PaymentMethod,
                    PaymentDate = p.PaymentDate,
                    Status = "Confirmed"
                }).ToList()


            };
        }

        public CustomerpDTO GetCustomerByUserId(int userId)
        {
            var c = repo.GetCustomerByUserId(userId);

            return new CustomerpDTO
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email
            };
        }

        // ROOM SEARCH (Advanced availability search placeholder)
        public List<RoomAvailabilityDTO> SearchAvailableRooms(DateTime checkIn, DateTime checkOut, string type = null, decimal? min = null, decimal? max = null)
        {
            var rooms = repo.GetRooms();

            // You will add advanced overlap-check LINQ here later (F1)
            var filtered = rooms.Where(r => r.Status == "Available");

            if (!string.IsNullOrEmpty(type))
                filtered = filtered.Where(r => r.RoomType.TypeName.Contains(type));

            if (min.HasValue) filtered = filtered.Where(r => r.Price >= min.Value);
            if (max.HasValue) filtered = filtered.Where(r => r.Price <= max.Value);

            return filtered.Select(r => new RoomAvailabilityDTO
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                TypeName = r.RoomType.TypeName,
                Price = r.Price,
                Status = r.Status
            }).ToList();
        }

        // MAKE RESERVATION
        public bool BookRoom(int customerId, int roomId, DateTime checkIn, DateTime checkOut, decimal pricePerNight, string paymentMethod)
        {
            int nights = (checkOut - checkIn).Days;
            decimal total = nights * pricePerNight; // F2

            var res = new DAL.EF.Tables.Reservation
            {
                RoomId = roomId,
                CustomerId = customerId,
                CheckInDate = DateOnly.FromDateTime(checkIn),
                CheckOutDate = DateOnly.FromDateTime(checkOut),
                TotalAmount = total,
                Status = "Confirmed"
            };

            var success = repo.AddReservation(res);
            if (!success) return false;

            repo.AddPayment(new DAL.EF.Tables.Payment
            {
                ReservationId = res.ReservationId,
                Amount = total,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now
            });



            return true;
        }


    }
}