using System;
using System.Collections.Generic;

namespace Inventarium.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int? RoomNumber { get; set; }

    public virtual ICollection<Movement> MovementSourceRooms { get; set; } = new List<Movement>();

    public virtual ICollection<Movement> MovementTargetRooms { get; set; } = new List<Movement>();
}
