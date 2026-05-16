using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            // In a real app, get these from the logged-in User's Session/Claims
            int employeeId = 1;
            string employeeName = "John Doe";
            string approvalStatus = "Approved"; // Change to "Pending" to test the UI alert

            var model = _employeeService.GetDashboardData(employeeId, employeeName, approvalStatus);
            return View(model);
        }

        [HttpPost]
        public IActionResult MarkRoomCleaned(int roomId)
        {
            _employeeService.MarkRoomAvailable(roomId);
            TempData["SuccessMessage"] = "Room marked as Available!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult ReportMaintenance(int roomId)
        {
            _employeeService.ReportMaintenanceIssue(roomId);
            TempData["WarningMessage"] = "Room reported for Maintenance.";
            return RedirectToAction("Dashboard");
        }
    }
}