using System;
using System.Collections.Generic;

namespace WhoKnows_backend.Models;

public partial class Page
{
    public string Title { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Language { get; set; } = null!;

    public DateTime? LastUpdated { get; set; }

    public string Content { get; set; } = null!;

    public int Id { get; set; }
}
