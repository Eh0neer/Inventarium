using System;
using System.Collections.Generic;

namespace Inventarium.Models;

public partial class Movement
{
    public int MovementId { get; set; }

    public int? EmployeeId { get; set; }

    public int? EquipmentId { get; set; }

    public int? SourceRoomId { get; set; }

    public int? TargetRoomId { get; set; }

    public DateOnly? MovementDate { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Equipment? Equipment { get; set; }

    public virtual Room? SourceRoom { get; set; }

    public virtual Room? TargetRoom { get; set; }
}
