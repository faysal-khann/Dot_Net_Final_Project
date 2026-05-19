using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly ReceptionistService service;
        public ReceptionistController(ReceptionistService service) { this.service = service; }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") == "employee")
            {
                var model = service.GetDashboard();
                return View(model);
            }
            TempData["ErrorMessage"] = "Unauthorized access. Please log in to view this page.";
            return RedirectToAction("Login", "Auth");

        }

        public IActionResult Reservations() => View(service.GetReservations());

        public IActionResult CreateReservation() => View();

        [HttpPost]
        public IActionResult CreateReservation(ReservationDTO dto)
        {
            service.CreateReservation(dto);
            return RedirectToAction("Dashboard");
        }

        

        public IActionResult CancelReservation(int id)
        {
            service.CancelReservation(id);
            return RedirectToAction("Dashboard");
        }

        public IActionResult CheckIn(int id)
        {
            service.CheckIn(id);
            return RedirectToAction("Dashboard");
        }

        public IActionResult CheckOut(int id)
        {
            var invoice = service.GenerateInvoice(id);
            return View("CheckOut", invoice);
        }

        [HttpPost]
        public IActionResult ConfirmCheckOut(int id, string paymentMethod)
        {
            service.CheckOut(id, paymentMethod);
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public IActionResult EditReservation(int id)
        {
            var model = service.GetReservation(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public IActionResult EditReservation(ReservationDTO dto)
        {
            service.UpdateReservation(dto);
            return RedirectToAction("Dashboard");
        }
        public IActionResult SearchReservations(string CustomerName, DateTime? Date, string Status)
        {
            // Re-use the dashboard data but replace the Reservations list with the filtered one
            var model = service.GetDashboard();
            model.Reservations = service.SearchReservations(CustomerName, Date, Status);

            // Return to the Dashboard view with the filtered model
            return View("Dashboard", model);
        }
        [HttpGet]
        public IActionResult DeleteReservation(int id)
        {
            service.DeleteReservation(id);
            return RedirectToAction("Dashboard");
        }
    }
}