using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class HomePage
{
    public decimal Id { get; set; }

    public string? LogoPath { get; set; }

    public string? HeaderComponent1 { get; set; }

    public string? HeaderComponent2 { get; set; }

    public string? FooterComponent1 { get; set; }

    public string? FooterComponent2 { get; set; }

    public string? ImagePath1 { get; set; }

    public string? ImagePath2 { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }
}
