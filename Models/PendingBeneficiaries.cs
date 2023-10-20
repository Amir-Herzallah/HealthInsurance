using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace HealthInsurance.Models
{
    public class PendingBeneficiary
    {
        public decimal Id { get; set; }

        public decimal? Subscriptionid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string RelationshipToSubscriber { get; set; }

        [Required]
        public string Status { get; set; } // Status can be "Pending," "Accepted," "Rejected," etc.

        public string BeneficiaryImagePath { get; set; }

        [NotMapped]
        public IFormFile BeneficiaryImageFile { get; set; }

        public DateTime BeneficiaryCreationDate { get; set; }
    }
}
