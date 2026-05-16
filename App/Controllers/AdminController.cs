using BLL.Services;
using Microsoft.AspNetCore.Mvc;

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
            var stats = _adminService.GetDashboardStats();

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
