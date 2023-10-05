using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class ContactUs
{
    public decimal Id { get; set; }

    public string? LogoPath { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public string? Subject { get; set; }
}
