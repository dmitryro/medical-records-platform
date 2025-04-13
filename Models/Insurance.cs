using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("insurances", Schema = "public")]
    public class Insurance
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("contact_info")]
        public string ContactInfo { get; set; }

        [Column("coverage_details")]
        public string? CoverageDetails { get; set; }

        [Required]
        [Column("claims_integration_status")]
        public string ClaimsIntegrationStatus { get; set; }
    }
}
