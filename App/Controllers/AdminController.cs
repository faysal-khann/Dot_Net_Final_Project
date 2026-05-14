using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

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
            // TODO: Call _adminService.GetDashboardStats() to get real DB data
            var stats = new BLL.DTOs.DashboardDTO
            {
                RevenueToday = 1250.00m,
                RevenueThisMonth = 34500.00m,
                OccupancyRate = 82.5,
                ConfirmedReservations = 45,
                PendingReservations = 12,
                CancelledReservations = 3,
                PendingEmployeeApprovals = 2 // Links to the approval page we just built!
            };

            return View(stats);


        }
        [HttpGet]
        public IActionResult PendingEmployees()
        {
            var data = _adminService.GetPendingEmployees();
            return View(data);
        }

        // POST: Approve
        [HttpPost]
        public IActionResult Approve(int id)
        {
            var success = _adminService.ApproveEmployee(id);
            if (success)
            {
                TempData["Message"] = "Employee Approved Successfully!";
            }
            return RedirectToAction("PendingEmployees");
        }

        // POST: Reject
        [HttpPost]
        public IActionResult Reject(int id)
        {
            var success = _adminService.RejectEmployee(id);
            if (success)
            {
                TempData["Message"] = "Employee Rejected.";
            }
            return RedirectToAction("PendingEmployees");
        }
    }
}
