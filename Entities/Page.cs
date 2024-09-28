using System;
using System.Collections.Generic;

namespace WhoKnows_backend.Entities;

public partial class Page
{
    public string Title { get; set; } = null!;

    public int? Id { get; set; }

    public string? Language { get; set; }

    public string Content { get; set; } = null!;

    public string Url { get; set; } = null!;
}
