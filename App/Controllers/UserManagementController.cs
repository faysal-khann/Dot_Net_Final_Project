using Microsoft.AspNetCore.Mvc;
using BLL.Services;

namespace App.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManagementService userService;

        public UserManagementController(UserManagementService userService)
        {
            this.userService = userService;
        }

        // GET: View all users (CRUD: Read)
        [HttpGet]
        public IActionResult Index()
        {
            var users = userService.GetAllUsers();
            return View(users);
        }

        // GET: Assign Role form (BEYOND CRUD)
        [HttpGet]
        public IActionResult AssignRole(int id)
        {
            var user = userService.GetUserById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: Process Role Assignment
        [HttpPost]
        public IActionResult AssignRole(int UserId, string Role)
        {
            var success = userService.AssignRole(UserId, Role);
            if (success)
            {
                TempData["Success"] = "Role assigned successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to assign role.";
            }
            return RedirectToAction("Index");
        }
       
        
        // POST: Process Role Assignment
        [HttpPost]
        public IActionResult ChangeStatus(int userId, string status)
        {
            var success = userService.ChangeStatus(userId, status);

            if (success)
                TempData["Success"] = "Status updated successfully!";
            else
                TempData["Error"] = "Failed to update status.";

            return RedirectToAction("Index");
        }

        // POST: Delete User (CRUD: Delete)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            userService.DeleteUser(id);
            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}