using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class CustomerpDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsVIP { get; set; }
    }
}
