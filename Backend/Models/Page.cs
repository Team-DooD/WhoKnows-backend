using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoKnows_backend.Models;

public partial class Page
{
    public string Title { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string Language { get; set; } = null!;

    public DateTime? LastUpdated { get; set; }

    public string Content { get; set; } = null!;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
}