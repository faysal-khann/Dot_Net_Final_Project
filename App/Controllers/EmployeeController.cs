using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService service;

        public EmployeeController(EmployeeService employeeService)
        {
            this.service = employeeService;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            // Simulate logged in User. Replace with: int userId = int.Parse(User.FindFirst("UserId").Value);
            int? userId = HttpContext.Session.GetInt32("UserId");
            var employee = service.GetEmployeeByUserId(userId.Value);
            var model = service.GetDashboardData(userId.Value);

            if (model == null) return Content("Employee Profile not found.");

            return View(model);
        }

        [HttpPost]
        public IActionResult MarkRoomCleaned(int roomId)
        {
            service.MarkRoomAvailable(roomId);
            TempData["SuccessMessage"] = "Room marked as Available!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult MarkMaintenanceDone(int roomId)
        {
            service.MarkRoomAvailable(roomId); // Available after fix
            TempData["SuccessMessage"] = "Maintenance complete. Room is Available!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ReportMaintenance(int roomId)
        {
            service.ReportMaintenanceIssue(roomId);
            TempData["WarningMessage"] = "Room reported for Maintenance.";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ApproveEmployee(int employeeId)
        {
            service.ApproveEmployee(employeeId);
            TempData["SuccessMessage"] = "Employee Approved successfully!";
            return RedirectToAction("Dashboard");
        }
    }
}