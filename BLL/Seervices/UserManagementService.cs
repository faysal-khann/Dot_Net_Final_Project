using AutoMapper;
using BLL.DTOs;
using DAL.Repos;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class UserManagementService
    {
        UserManagementRepo repo;
        Mapper mapper;

        public UserManagementService(UserManagementRepo repo)
        {
            this.repo = repo;
            this.mapper = MapperConfig.GetMapper();
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = repo.GetAllUsers();
            return mapper.Map<List<UserDTO>>(users);
        }

        public UserDTO GetUserById(int id)
        {
            var user = repo.GetUserById(id);

            if (user == null)
                return null;

            return mapper.Map<UserDTO>(user);
        }

        public bool AssignRole(int userId, string role)
        {
            return repo.AssignRole(userId, role);
        }
        public bool ChangeStatus(int userId, string s)
        {
            return repo.ChangeStatus(userId, s);
        }

        public bool DeleteUser(int id)
        {
            return repo.DeleteUser(id);
        }
    }
}