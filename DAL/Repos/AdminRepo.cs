using DAL.EF;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Needed for .Include()

namespace DAL.Repos
{
    public class AdminRepo
    {
        private readonly HotelManagementContext db;

        public AdminRepo(HotelManagementContext db)
        {
            this.db = db;
        }

        // 1. Get all employees waiting for approval
        public List<Employee> GetPendingEmployees()
        {
            return db.Employees
                     .Include(e => e.User) // Join with User table to get Email/Name
                     .Where(e => e.ApprovalStatus == "pending")
                     .ToList();
        }

        // 2. Change Approval Status and update User Login Status
        // 1. Approve & Set Salary
        public bool ApproveEmployee(int employeeId, decimal newSalary)
        {
            var employee = db.Employees.Find(employeeId);

            if (employee == null)
            {
                return false;
            }

            var user = db.Users.Find(employee.UserId);

            employee.ApprovalStatus = "approved";
            employee.Salary = newSalary;


            if (user != null)
            {
                user.Status = "active";
            }

            return db.SaveChanges() > 0;
        }

        // 2. Reject Employee
        public bool RejectEmployee(int employeeId)
        {
            var employee = db.Employees.Find(employeeId);

            if (employee == null)
            {
                return false;
            }

            var user = db.Users.Find(employee.UserId);

            employee.ApprovalStatus = "rejected";

            if (user != null)
            {
                user.Status = "inactive";
            }

            return db.SaveChanges() > 0;
        }




        //------------------------------------------- -------------------------------------------
        public decimal GetRevenueToday()
        {
            var today = DateTime.Today;

            return db.Payments
                .Where(p => p.PaymentDate.Date == today)
                .Sum(p => (decimal?)p.Amount) ?? 0m;
        }

        public decimal GetRevenueThisMonth()
        {
            var today = DateTime.Today;
            // Sum payments matching the current month and year
            return db.Payments
                     .Where(p => p.PaymentDate.Month == today.Month && p.PaymentDate.Year == today.Year)
                     .Sum(p => (decimal?)p.Amount) ?? 0m;
        }

        public double GetOccupancyRate()
        {
            int totalRooms = db.Rooms.Count();
            if (totalRooms == 0) return 0;

            int occupiedRooms = db.Rooms.Count(r => r.Status == "Occupied");

            // Calculate percentage and round to 2 decimal places
            return Math.Round((double)occupiedRooms / totalRooms * 100, 2);
        }

        public int GetReservationCountByStatus(string status)
        {
            return db.Reservations.Count(r => r.Status == status);
        }

        public int GetPendingEmployeeApprovals()
        {
            // Count employees waiting for admin approval
            return db.Employees.Count(e => e.ApprovalStatus == "Pending");
        }

        // Add this inside AdminDashboardRepo.cs


            public List<Payment> GetAllPaymentsWithDetails()
            {
                   return db.Payments
                 .Include(p => p.Reservation).ThenInclude(r => r.Customer)
                 .Include(p => p.Reservation).ThenInclude(r => r.Room)
                 .OrderByDescending(p => p.PaymentDate)
                 .ToList();
             }

}
}