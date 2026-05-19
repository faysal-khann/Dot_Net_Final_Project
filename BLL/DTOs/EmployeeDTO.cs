namespace BLL.DTOs
{
    public class EmployeeDTO
    {
        // Employee table fields


        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string Position { get; set; } = null!;
        public string Password { get; set; } = null!;
        
        

        public decimal Salary { get; set; }

        public string Phone { get; set; } = null!;
        public DateTime HiredAt { get; set; }

        // Related user fields
        
    }
}