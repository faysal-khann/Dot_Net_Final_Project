using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class CustomerDTO
    {


        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        // Customer table fields
        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
