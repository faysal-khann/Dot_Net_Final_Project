using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace App.Controllers
{
    public class AdminController : Controller
    {
        AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            // Fetch real DB data!
            if (HttpContext.Session.GetString("Role") == "admin")
            {
                var stats = _adminService.GetDashboardStats();

                return View(stats);
            }
            TempData["ErrorMessage"] = "Unauthorized access. Please log in to view this page.";
            return RedirectToAction("Login", "Auth");

        }
        [HttpGet]
        public IActionResult PendingEmployees()
        {
            var data = _adminService.GetPendingEmployees();
            return View(data);
        }

        // POST: Approve
        [HttpPost]
        public IActionResult Approve(int id, decimal newSalary)
        {
            bool success = _adminService.ApproveEmployeeWithSalary(id, newSalary);

            if (success)
            {
                TempData["Success"] = "Employee approved and salary updated successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to approve employee. They might not exist.";
            }

            // Redirect back to the page that lists the employees
            return RedirectToAction("PendingEmployees"); // Change this to match your actual view name
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            bool success = _adminService.RejectEmployee(id);

            if (success)
            {
                TempData["Success"] = "Employee has been rejected.";
            }
            else
            {
                TempData["Error"] = "Failed to reject employee.";
            }

            // Redirect back to the page that lists the employees
            return RedirectToAction("PendingEmployees"); // Change this to match your actual view name
        }
        // Add this inside AdminController.cs
        [HttpGet]
        public IActionResult ViewPayments()
        {
            var payments = _adminService.GetAllPayments();
            return View(payments);
        }

        //------------------------------------------------------------------------------------------------------------

    }
}
