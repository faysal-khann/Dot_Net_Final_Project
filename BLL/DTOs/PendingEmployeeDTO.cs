using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class PendingEmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Position { get; set; } = null!;
        public decimal Salary { get; set; }
        public DateTime HiredAt { get; set; }
    }
}
