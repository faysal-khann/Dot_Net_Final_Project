using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int CustomerId { get; set; }

    public int RoomId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly CheckOutDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();

    public virtual Room Room { get; set; } = null!;
}
