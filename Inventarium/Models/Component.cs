using System;
using System.Collections.Generic;

namespace Inventarium.Models;

public partial class Component
{
    public int ComponentId { get; set; }

    public int? EquipmentId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? SerialNumber { get; set; }

    public virtual Equipment? Equipment { get; set; }
}
