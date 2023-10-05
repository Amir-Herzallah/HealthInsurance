using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Testimonials
{
    public decimal Id { get; set; }

    public decimal? Userid { get; set; }

    public decimal Rating { get; set; }

    public string? Commentt { get; set; }

    public DateTime SubmissionDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Users? User { get; set; }
}
