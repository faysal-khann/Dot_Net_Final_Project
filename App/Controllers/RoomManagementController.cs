using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class RoomManagementControlle : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
