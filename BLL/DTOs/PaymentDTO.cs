using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string CustomerName { get; set; } = null!;
        public string RoomNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }
    }
}
