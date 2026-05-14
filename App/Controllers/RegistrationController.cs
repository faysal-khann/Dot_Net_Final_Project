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
                {
                    var role = HttpContext.Session.GetString("Role");

                    if (role == "admin")
                    {
                        return RedirectToAction("Index", "UserManagement");
                    }

                    return RedirectToAction("Login", "Auth");
                }
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
                {
                    var role = HttpContext.Session.GetString("Role");

                    if (role == "admin")
                    {
                        return RedirectToAction("Index", "UserManagement");
                    }

                    return RedirectToAction("Login", "Auth");
                }
            }
            return View(dto);
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }
    }
}