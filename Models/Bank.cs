using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Bank
{
    public string CardNo { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string CardHolderName { get; set; } = null!;

    public string Cvv { get; set; } = null!;

    public decimal Balance { get; set; }
}
