using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class RoomManagementService  
    {
        private readonly RoomManagementRepo _repo;

        public RoomManagementService(RoomManagementRepo  repo) 
        { _repo = repo; }

        // --- Room Types Logic (MANUAL MAPPING) ---
        public List<RoomTypeDTO> GetRoomTypes()
        {
            return _repo.GetRoomTypes().Select(t => new RoomTypeDTO
            {
                RoomTypeId = t.RoomTypeId,
                TypeName = t.TypeName,
                Capacity = t.Capacity
            }).ToList();
        }
        public RoomTypeDTO GetRoomTypeById(int id)
        {
            var t = _repo.GetRoomTypeById(id);
            if (t == null) return null;
            return new RoomTypeDTO { RoomTypeId = t.RoomTypeId, TypeName = t.TypeName, Capacity = t.Capacity };
        }
        public bool AddRoomType(RoomTypeDTO dto) => _repo.AddRoomType(new RoomType { TypeName = dto.TypeName, Capacity = dto.Capacity });
        public bool EditRoomType(RoomTypeDTO dto) => _repo.UpdateRoomType(new RoomType { RoomTypeId = dto.RoomTypeId, TypeName = dto.TypeName, Capacity = dto.Capacity });
        public bool DeleteRoomType(int id) => _repo.DeleteRoomType(id);

        // --- Rooms Logic (MANUAL MAPPING) ---
        public RoomDTO GetRoomById(int id)
        {
            var r = _repo.GetRoomById(id);
            if (r == null) return null;
            return new RoomDTO { RoomId = r.RoomId, RoomNumber = r.RoomNumber, RoomTypeId = r.RoomTypeId, Price = r.Price, Status = r.Status };
        }
        public bool AddRoom(RoomDTO dto) => _repo.AddRoom(new Room { RoomNumber = dto.RoomNumber, RoomTypeId = dto.RoomTypeId, Price = dto.Price, Status = dto.Status });
        public bool EditRoom(RoomDTO dto) => _repo.UpdateRoom(new Room { RoomId = dto.RoomId, RoomNumber = dto.RoomNumber, RoomTypeId = dto.RoomTypeId, Price = dto.Price, Status = dto.Status });
        public bool DeleteRoom(int id) => _repo.DeleteRoom(id);

        public List<RoomDTO> GetRoomsFiltered(int? typeId, string status, decimal? minPrice, decimal? maxPrice)
        {
            return _repo.FilterRooms(typeId, status, minPrice, maxPrice).Select(r => new RoomDTO
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                RoomTypeId = r.RoomTypeId,
                RoomTypeName = r.RoomType.TypeName,
                Capacity = r.RoomType.Capacity,
                Price = r.Price,
                Status = r.Status
            }).ToList();
        }

        // --- Beyond CRUD Logic ---
        public int MarkFloorMaintenance(string floorPrefix) => _repo.BulkUpdateFloorStatus(floorPrefix, "Maintenance");
        public int SetPricingByType(int typeId, decimal price) => _repo.BulkUpdatePriceByType(typeId, price);
    }
}