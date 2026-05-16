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
            var model = service.GetDashboard();
            return View(model);
        }

        public IActionResult Reservations() => View(service.GetReservations());

        public IActionResult CreateReservation() => View();

        [HttpPost]
        public IActionResult CreateReservation(ReservationDTO dto)
        {
            service.CreateReservation(dto);
            return RedirectToAction("Dashboard");
        }

        public IActionResult EditReservation(int id)
        {
            return View(service.GetReservation(id));
        }

        [HttpPost]
        public IActionResult EditReservation(ReservationDTO dto)
        {
            service.UpdateReservation(dto);
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
    }
}