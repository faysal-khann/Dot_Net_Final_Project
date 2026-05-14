using AutoMapper;
using BLL.DTOs;
using DAL.Repos;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly AuthRepo repo;
        private readonly IMapper mapper;

        public AuthService(AuthRepo repo)
        {
            this.repo = repo;
            this.mapper = MapperConfig.GetMapper();
        }

        public UserDTO Authenticate(LoginDTO login)
        {
            var user = repo.Authenticate(login.Email, login.Password);
            if (user != null)
            {
                // Login successful, map entity to DTO and return
                return mapper.Map<UserDTO>(user);
            }
            return null; // Login failed
        }
    }
}