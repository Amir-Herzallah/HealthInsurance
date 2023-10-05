using System;
using System.Collections.Generic;

namespace HealthInsurance.Models;

public partial class Roles
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
