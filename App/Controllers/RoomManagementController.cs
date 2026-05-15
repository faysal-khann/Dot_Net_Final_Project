using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace App.Controllers
{
    public class RoomManagementController : Controller
    {
        private readonly RoomManagementService _roomService;

        public RoomManagementController(RoomManagementService roomService) { _roomService = roomService; }

        // GET: Dashboard / Filtered List
        public IActionResult Index(int? typeId, string status, decimal? minPrice, decimal? maxPrice)
        {
            ViewBag.RoomTypes = _roomService.GetRoomTypes();
            var rooms = _roomService.GetRoomsFiltered(typeId, status, minPrice, maxPrice);
            return View(rooms);
        }

        // --- ROOM CRUD ---
        [HttpGet]
        public IActionResult CreateRoom()
        {
            ViewBag.RoomTypes = _roomService.GetRoomTypes();
            // Add this line to fetch all existing rooms and pass them to the view
            ViewBag.ExistingRooms = _roomService.GetRoomsFiltered(null, null, null, null);

            return View();
        }
        [HttpPost] public IActionResult CreateRoom(RoomDTO dto) { _roomService.AddRoom(dto); return RedirectToAction("Index"); }

        [HttpGet] public IActionResult EditRoom(int id) { ViewBag.RoomTypes = _roomService.GetRoomTypes(); return View(_roomService.GetRoomById(id)); }
        [HttpPost] public IActionResult EditRoom(RoomDTO dto) { _roomService.EditRoom(dto); return RedirectToAction("Index"); }

        [HttpPost] public IActionResult DeleteRoom(int id) { _roomService.DeleteRoom(id); return RedirectToAction("Index"); }

        // --- ROOM TYPE CRUD ---
        [HttpGet] public IActionResult RoomTypes() => View(_roomService.GetRoomTypes());

        [HttpGet] public IActionResult CreateRoomType() => View();
        [HttpPost] public IActionResult CreateRoomType(RoomTypeDTO dto) { _roomService.AddRoomType(dto); return RedirectToAction("RoomTypes"); }

        [HttpGet] public IActionResult EditRoomType(int id) => View(_roomService.GetRoomTypeById(id));
        [HttpPost] public IActionResult EditRoomType(RoomTypeDTO dto) { _roomService.EditRoomType(dto); return RedirectToAction("RoomTypes"); }

        [HttpPost] public IActionResult DeleteRoomType(int id) { _roomService.DeleteRoomType(id); return RedirectToAction("RoomTypes"); }

        // --- BEYOND CRUD ACTIONS ---
        [HttpPost]
        public IActionResult MarkFloorMaintenance(string floor)
        {
            _roomService.MarkFloorMaintenance(floor);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SetPricingByType(int roomTypeId, decimal newPrice)
        {
            _roomService.SetPricingByType(roomTypeId, newPrice);
            return RedirectToAction("Index");
        }
    }
}