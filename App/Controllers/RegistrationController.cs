using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using BLL.DTOs;

namespace App.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly RegistrationService _regService;

        public RegistrationController(RegistrationService regService)
        {
            _regService = regService;
        }

        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterCustomer(CustomerDTO dto)
        {
            if (ModelState.IsValid)
            {
                var success = _regService.RegisterCustomer(dto);
                if (success)
                    return RedirectToAction("Login", "Auth");
                ViewBag.Error = "Registration failed. Try again.";
            }
            return View(dto);
        }

        [HttpGet]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterEmployee(EmployeeDTO dto)
        {
            if (ModelState.IsValid)
            {
                var success = _regService.RegisterEmployee(dto);
                if (success)
                    return RedirectToAction("Login","Auth" );
                ViewBag.Error = "Registration failed. Try again.";
            }
            return View(dto);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }
    }
}