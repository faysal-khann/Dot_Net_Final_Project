using BLL.DTOs;
using BLL.Services;
using DAL.EF.Tables;
using Microsoft.AspNetCore.Http; // Required for Session
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            HttpContext.Session.SetString("Role", "");
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
                        var position = _authService.GetEmployeePosition(user.UserId);
                        
                        if (user.Status == "active")
                        {

                            if (position == "Receptionist")
                            {
                                HttpContext.Session.SetString("position", "Receptionist");
                                return RedirectToAction("Dashboard", "Receptionist");
                            }
                            return RedirectToAction("Dashboard", "Employee");
                        } 
                        else if (user.Status == "pending")
                        {

                            ViewBag.Error = "Your account is pending. Please contact admin.";
                           
                        } 
                        
                        else
                        {
                            ViewBag.Error = "Your account is inactive. Please contact admin.";
                            
                        }
                        return View();
                    }
                    else if (user.Role == "admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (user.Role == "customer")

                    {
                        if (user.Status == "active")
                        {

                            return RedirectToAction("Dashboard", "Customer");
                        }
                        else if (user.Status == "pending")
                        {

                            ViewBag.Error = "Your account is pending. Please contact admin.";
                        }
                        else
                        {
                            ViewBag.Error = "Your account is inactive. Please contact admin.";
                        }
                        return View();

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