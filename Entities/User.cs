using System;
using System.Collections.Generic;

namespace WhoKnows_backend.Entities;

public partial class User
{
    public string Username { get; set; } = string.Empty;

    public string? Password { get; set; }

    public int Id { get; set; }

    public string Email { get; set; } = null!;
}
