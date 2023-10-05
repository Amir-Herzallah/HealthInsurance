using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Subscriptions
{
    public decimal Id { get; set; }

    public decimal? Userid { get; set; }

    public DateTime StartDate { get; set; }

    public decimal Amount { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Beneficiaries> Beneficiaries { get; set; } = new List<Beneficiaries>();

    public virtual Users? User { get; set; }
}
