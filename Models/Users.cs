using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Models;

public partial class Users
{
    public decimal Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime RegistrationDate { get; set; }

    public decimal? Roleid { get; set; }

    public string? ProfilePictureUrl { get; set; }

    [NotMapped]
    public IFormFile? ProfilePictureFile { get; set; }

    public virtual Roles? Role { get; set; }

    public virtual Subscriptions? Subscription { get; set; }

    public virtual ICollection<Testimonials> Testimonials { get; set; } = new List<Testimonials>();
}
