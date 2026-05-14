using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
