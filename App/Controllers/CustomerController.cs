using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace App.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService service;
        public CustomerController(CustomerService service) { this.service = service; }

        public IActionResult Dashboard()
        {
            int customerId = 4; // In a real app, get this from the logged-in User's Session/Claims
            var model = service.GetDashboard(customerId);
            return View(model);
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
            int customerId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            service.BookRoom(customerId, roomId, checkIn, checkOut, price, method);
            return RedirectToAction("Dashboard");
        }
    }
}