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
        public bool ProcessEmployeeApproval(int employeeId, string newStatus)
        {
            var employee = db.Employees.Find(employeeId);
            if (employee == null) return false;

            // Update Employee Table
            employee.ApprovalStatus = newStatus;

            // Workflow Automation: If Approved, activate their User account
            var user = db.Users.Find(employee.UserId);
            if (user != null)
            {
                if (newStatus == "Approved")
                {
                    user.Status = "active"; // Now they can log in
                }
                else if (newStatus == "Rejected")
                {
                    user.Status = "inactive"; // Prevent login
                }
            }

            return db.SaveChanges() > 0;
        }




        //-------------------------------------------User management methods -------------------------------------------
    }
}