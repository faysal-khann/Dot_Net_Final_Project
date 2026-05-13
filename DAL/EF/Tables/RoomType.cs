using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
