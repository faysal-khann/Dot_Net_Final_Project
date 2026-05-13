using System;
using System.Collections.Generic;

namespace DAL.EF.Tables;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    public decimal Salary { get; set; }

    public string Phone { get; set; } = null!;

    public string ApprovalStatus { get; set; } = null!;

    public DateTime HiredAt { get; set; }

    public virtual User User { get; set; } = null!;
}
