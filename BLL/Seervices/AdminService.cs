using AutoMapper;
using BLL.DTOs;
using DAL.Repos;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class AdminService
    {
        public  AdminRepo repo;
        Mapper mapper;

        public AdminService(AdminRepo repo)
        {
            this.repo = repo;
            this.mapper = MapperConfig.GetMapper();
        }

        public List<PendingEmployeeDTO> GetPendingEmployees()
        {
            var pending = repo.GetPendingEmployees();

            // Mapping Entity to DTO (Manual mapping used here for clarity with Joins, 
            // but you can add this to AutoMapper if you prefer)
            var dtos = pending.Select(e => new PendingEmployeeDTO
            {
                EmployeeId = e.EmployeeId,
                Name = e.Name,
                Email = e.User.Email, // Getting Email from joined User table!
                Position = e.Position,
                Salary = e.Salary,
                HiredAt = e.HiredAt
            }).ToList();

            return dtos;
        }

        public bool ApproveEmployee(int employeeId)
        {
            return repo.ProcessEmployeeApproval(employeeId, "Approved");
        }

        public bool RejectEmployee(int employeeId)
        {
            return repo.ProcessEmployeeApproval(employeeId, "Rejected");
        }
        //-----------------------------------------------------------------------------------------------------------
        public DashboardDTO GetDashboardStats()
        {
            // Gather all the real data from the DAL and put it into the DTO
            var stats = new DashboardDTO
            {
                RevenueToday = repo.GetRevenueToday(),
                RevenueThisMonth = repo.GetRevenueThisMonth(),
                OccupancyRate = repo.GetOccupancyRate(),

                ConfirmedReservations = repo.GetReservationCountByStatus("Confirmed"),
                PendingReservations = repo.GetReservationCountByStatus("Pending"),
                CancelledReservations = repo.GetReservationCountByStatus("Cancelled"),

                PendingEmployeeApprovals = repo.GetPendingEmployeeApprovals()
            };

            return stats;
        }
        // Add this inside AdminDashboardService.cs
        public List<PaymentDTO> GetAllPayments()
        {
            var payments = repo.GetAllPaymentsWithDetails();

            return payments.Select(p => new PaymentDTO
            {
                PaymentId = p.PaymentId,
                ReservationId = p.ReservationId,
                Amount = p.Amount,
                Method = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                CustomerName = p.Reservation.Customer.Name,
                RoomNumber = p.Reservation.Room.RoomNumber
            }).ToList();
        }

    }
}