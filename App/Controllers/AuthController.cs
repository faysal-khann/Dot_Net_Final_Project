using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using BLL.DTOs;
using Microsoft.AspNetCore.Http; // Required for Session

namespace App.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Authenticate(loginDTO);

                if (user != null)
                {
                    // Store user details in Session
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetString("Name", user.Name);

                    // Redirect based on Role
                    if (user.Role == "employee")
                    {
                        if(user.Status == "active")
                        {
                            return RedirectToAction("Dashboard", "Employee");
                        }
                        else
                        {
                            ViewBag.Error = "Your account is inactive. Please contact admin.";
                        }
                    }
                    else if (user.Role == "admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (user.Role == "receptionist")
                    {
                        return RedirectToAction("Dashboard", "Customer");
                    }
                    else
                    {
                        //return RedirectToAction("Dashboard", "Admin");
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.Error = "Invalid Email or Password";
            }
            return View(loginDTO);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears the session
            return RedirectToAction("Login");
        }
    }
}