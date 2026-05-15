using DAL.EF;
using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repos
{
    public class RoomManagementRepo
    {
        private readonly HotelManagementContext db;

        public RoomManagementRepo(HotelManagementContext db) { this.db = db; }

        // --- CRUD: Room Types ---
        public List<RoomType> GetRoomTypes() => db.RoomTypes.ToList();
        public RoomType GetRoomTypeById(int id) => db.RoomTypes.Find(id);
        public bool AddRoomType(RoomType rt) { db.RoomTypes.Add(rt); return db.SaveChanges() > 0; }
        public bool UpdateRoomType(RoomType rt)
        {
            var existing = db.RoomTypes.Find(rt.RoomTypeId);
            if (existing == null) return false;
            existing.TypeName = rt.TypeName;
            existing.Capacity = rt.Capacity;
            return db.SaveChanges() > 0;
        }
        public bool DeleteRoomType(int id) { var rt = db.RoomTypes.Find(id); if (rt != null) { db.RoomTypes.Remove(rt); return db.SaveChanges() > 0; } return false; }

        // --- CRUD: Rooms ---
        public Room GetRoomById(int id) => db.Rooms.Include(r => r.RoomType).FirstOrDefault(r => r.RoomId == id);
        public bool AddRoom(Room r) { db.Rooms.Add(r); return db.SaveChanges() > 0; }
        public bool UpdateRoom(Room r)
        {
            var existing = db.Rooms.Find(r.RoomId);
            if (existing == null) return false;
            existing.RoomNumber = r.RoomNumber;
            existing.RoomTypeId = r.RoomTypeId;
            existing.Price = r.Price;
            existing.Status = r.Status;
            return db.SaveChanges() > 0;
        }
        public bool DeleteRoom(int id) { var r = db.Rooms.Find(id); if (r != null) { db.Rooms.Remove(r); return db.SaveChanges() > 0; } return false; }

        // --- BEYOND CRUD #1: Advanced Filtering ---
        public List<Room> FilterRooms(int? typeId, string status, decimal? minPrice, decimal? maxPrice)
        {
            var query = db.Rooms.Include(r => r.RoomType).AsQueryable();

            if (typeId.HasValue && typeId.Value > 0) query = query.Where(r => r.RoomTypeId == typeId.Value);
            if (!string.IsNullOrEmpty(status)) query = query.Where(r => r.Status == status);
            if (minPrice.HasValue) query = query.Where(r => r.Price >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(r => r.Price <= maxPrice.Value);

            return query.ToList();
        }

        // --- BEYOND CRUD #2: Bulk Status Update (Mark Floor as Maintenance) ---
        public int BulkUpdateFloorStatus(string floorPrefix, string newStatus)
        {
            var rooms = db.Rooms.Where(r => r.RoomNumber.StartsWith(floorPrefix)).ToList();
            foreach (var room in rooms) room.Status = newStatus;
            return db.SaveChanges();
        }

        // --- BEYOND CRUD #3: Set Pricing per Room Type (Bulk Update) ---
        public int BulkUpdatePriceByType(int roomTypeId, decimal newPrice)
        {
            var rooms = db.Rooms.Where(r => r.RoomTypeId == roomTypeId).ToList();
            foreach (var room in rooms) room.Price = newPrice;
            return db.SaveChanges();
        }
    }
}