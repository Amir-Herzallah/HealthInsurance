using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsurance.Models;

public partial class Beneficiaries
{
    public decimal Id { get; set; }

    public decimal? Subscriptionid { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string RelationshipToSubscriber { get; set; } = null!;

    public string? Status { get; set; }

    public string? BeneficiaryImagePath { get; set; }

    [NotMapped]
    public IFormFile? BeneficiaryImageFile { get; set; }

    public DateTime? BeneficiaryCreationDate { get; set; }

    public virtual Subscriptions? Subscription { get; set; }
}
