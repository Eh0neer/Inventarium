﻿namespace Inventarium.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string? Password { get; set; }

    public string Salt { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
