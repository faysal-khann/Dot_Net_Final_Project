using BLL.DTOs;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class ReceptionistService
    {
        private readonly ReceptionistRepo repo;
        public ReceptionistService(ReceptionistRepo repo) { this.repo = repo; }

        // Dashboard
        public ReceptionistDashboardDTO GetDashboard()
        {
            var reservations = GetReservations();
            var rooms = GetRoomAvailability();
            var payments = GetPayments();

            return new ReceptionistDashboardDTO
            {
                Reservations = reservations,
                Customers = GetCustomers(),
                Rooms = rooms,
                Payments = payments,

                TodayCheckIns = reservations.Count(r => r.CheckInDate.Date == DateTime.Today),
                TodayCheckOuts = reservations.Count(r => r.CheckOutDate.Date == DateTime.Today),
                RevenueToday = payments.Where(p => p.PaymentDate.Date == DateTime.Today).Sum(p => p.Amount),
                RoomsToClean = rooms.Count(r => r.Status == "Cleaning"),
                CleaningAlerts = rooms.Where(r => r.Status == "Cleaning").ToList()
            };
        }

        // Reservation CRUD
        public List<ReservationDTO> GetReservations() =>
            repo.GetAllReservations().Select(r => new ReservationDTO
            {
                ReservationId = r.ReservationId,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                CheckInDate = r.CheckInDate.ToDateTime(TimeOnly.MinValue),
                CheckOutDate = r.CheckOutDate.ToDateTime(TimeOnly.MinValue),
                Status = r.Status,
                TotalAmount = r.TotalAmount
            }).ToList();

        public ReservationDTO GetReservation(int id)
        {
            var r = repo.GetReservation(id);
            if (r == null) return null;

            return new ReservationDTO
            {
                ReservationId = r.ReservationId,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                CheckInDate = r.CheckInDate.ToDateTime(TimeOnly.MinValue),
                CheckOutDate = r.CheckOutDate.ToDateTime(TimeOnly.MinValue),
                Status = r.Status,
                TotalAmount = r.TotalAmount,
                Rooms = r.ReservationRooms.Select(rr => new ReservationRoomDTO
                {
                    RoomId = rr.RoomId,
                    RoomNumber = rr.Room.RoomNumber,
                    PricePerNight = (decimal)(rr.PricePerNight ?? 0)
                }).ToList()
            };
        }

        public bool CreateReservation(ReservationDTO dto)
        {
            int nights = (dto.CheckOutDate - dto.CheckInDate).Days;
            dto.TotalAmount = dto.Rooms.Sum(r => r.PricePerNight * nights); // Auto Total (extra)

            var res = new DAL.EF.Tables.Reservation
            {
                CustomerId = dto.CustomerId,
                CheckInDate = DateOnly.FromDateTime(dto.CheckInDate),
                CheckOutDate = DateOnly.FromDateTime(dto.CheckOutDate),
                Status = "Pending",
                TotalAmount = dto.TotalAmount
            };

            return repo.AddReservation(res);
        }

        public bool UpdateReservation(ReservationDTO dto)
        {
            var res = repo.GetReservation(dto.ReservationId);
            if (res == null) return false;

            res.CheckInDate = DateOnly.FromDateTime(dto.CheckInDate);
            res.CheckOutDate = DateOnly.FromDateTime(dto.CheckOutDate);
            res.Status = dto.Status;
            res.TotalAmount = dto.TotalAmount;

            return repo.UpdateReservation(res);
        }

        public bool CancelReservation(int id)
        {
            var res = repo.GetReservation(id);
            if (res == null) return false;
            res.Status = "Cancelled";
            return repo.UpdateReservation(res);
        }

        // Check-In/Check-Out
        public bool CheckIn(int reservationId)
        {
            var res = repo.GetReservation(reservationId);
            if (res == null) return false;

            res.Status = "Active";
            foreach (var rr in res.ReservationRooms)
                rr.Room.Status = "Occupied";

            return repo.UpdateReservation(res);
        }

        public InvoiceDTO GenerateInvoice(int reservationId)
        {
            var res = repo.GetReservation(reservationId);
            int nights = res.CheckOutDate.DayNumber - res.CheckInDate.DayNumber;

            return new InvoiceDTO
            {
                ReservationId = res.ReservationId,
                CustomerName = res.Customer.Name,
                Nights = nights,
                Amount = res.ReservationRooms.Sum(r => (decimal)(r.PricePerNight ?? 0) * nights),
                Rooms = res.ReservationRooms.Select(rr => new ReservationRoomDTO
                {
                    RoomNumber = rr.Room.RoomNumber,
                    PricePerNight = (decimal)(rr.PricePerNight ?? 0)
                }).ToList()
            };
        }

        public bool CheckOut(int reservationId, string method)
        {
            var res = repo.GetReservation(reservationId);
            if (res == null) return false;

            res.Status = "Completed";
            foreach (var rr in res.ReservationRooms)
                rr.Room.Status = "Cleaning"; // Notify housekeeping

            var invoice = GenerateInvoice(reservationId);
            repo.AddPayment(new DAL.EF.Tables.Payment
            {
                ReservationId = reservationId,
                Amount = invoice.Amount,
                PaymentMethod = method,
                PaymentDate = DateTime.Now
            });

            return repo.UpdateReservation(res);
        }

        // Customers
        public List<CustomersDTO> GetCustomers() =>
            repo.GetCustomers().Select(c => new CustomersDTO
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                IsVIP = false // VIP logic later
            }).ToList();

        // Rooms
        public List<RoomAvailabilityDTO> GetRoomAvailability() =>
            repo.GetRooms().Select(r => new RoomAvailabilityDTO
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                TypeName = r.RoomType.TypeName,
                Price = r.Price,
                Status = r.Status
            }).ToList();

        // Payments
        public List<PaymentDTO> GetPayments() =>
            repo.GetPayments().Select(p => new PaymentDTO
            {
                PaymentId = p.PaymentId,
                ReservationId = p.ReservationId,
                Amount = p.Amount,
                Method = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                Status = "Paid"
            }).ToList();
    }
}