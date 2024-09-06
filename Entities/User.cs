using System;
using System.Collections.Generic;

namespace WhoKnows_backend.Entities;

public partial class User
{
    public int Username { get; set; }

    public string? Password { get; set; }

    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;
}
