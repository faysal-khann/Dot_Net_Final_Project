using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace App.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService service;
        public CustomerController(CustomerService service) { this.service = service; }

        public IActionResult Dashboard(DateTime? checkIn, DateTime? checkOut, string type, decimal? minPrice, decimal? maxPrice)
        {
            
            if (HttpContext.Session.GetString("Role") == "customer")
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                var customer = service.GetCustomerByUserId(userId.Value);

                int customerId = customer.CustomerId;
                ViewBag.CustomerId = customerId;

                // In a real app, get this from the logged-in User's Session

                // 1. Set default dates if they just logged in
                var inDate = checkIn ?? DateTime.Today;
                var outDate = checkOut ?? DateTime.Today.AddDays(1);

                ViewBag.CheckIn = inDate.ToString("yyyy-MM-dd");
                ViewBag.CheckOut = outDate.ToString("yyyy-MM-dd");
                ViewBag.Type = type;
                ViewBag.MinPrice = minPrice;
                ViewBag.MaxPrice = maxPrice;

                // 2. Get the dashboard history
                var model = service.GetDashboard(customerId);

                // 3. Get the rooms and attach them to the dashboard model!
                model.AvailableRooms = service.SearchAvailableRooms(inDate, outDate, type, minPrice, maxPrice);

                return View(model);
            }
            TempData["ErrorMessage"] = "Unauthorized access. Please log in to view this page.";
            return RedirectToAction("Login", "Auth");

        }

        [HttpGet]
        public IActionResult SearchRooms(DateTime checkIn, DateTime checkOut, string type, decimal? minPrice, decimal? maxPrice)
        {
            var rooms = service.SearchAvailableRooms(checkIn, checkOut, type, minPrice, maxPrice);
            return View("AvailableRooms", rooms);
        }

        [HttpPost]
        public IActionResult BookRoom(int roomId, DateTime checkIn, DateTime checkOut, decimal price, string method)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            var customer = service.GetCustomerByUserId(userId.Value);

            int customerId = customer.CustomerId;

            service.BookRoom(customerId, roomId, checkIn, checkOut, price, method);
            return RedirectToAction("Dashboard");
        }
        // Add this inside CustomerController.cs
        [HttpPost]
        public IActionResult CancelReservation(int reservationId)
        {
            bool isCancelled = service.CancelReservation(reservationId);

            if (isCancelled)
            {
                TempData["Success"] = "Your reservation has been cancelled successfully.";
            }
            else
            {
                TempData["Error"] = "Could not cancel the reservation. Please contact support.";
            }

            // Redirect back to the dashboard to refresh the table
            return RedirectToAction("Dashboard");
        }
    }
}