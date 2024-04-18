using System;
using System.Collections.Generic;

namespace Inventarium.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Ipadres { get; set; }

    public string? SerialNumber { get; set; }

    public virtual ICollection<Component> Components { get; set; } = new List<Component>();

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
}
