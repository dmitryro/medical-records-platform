using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("billing_addresses", Schema = "public")]
    public class BillingAddress
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("patient_id")]
        public int? PatientId { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("state")]
        public string? State { get; set; }

        [Column("postal_code")]
        public string? PostalCode { get; set; }

        [Column("country")]
        public string? Country { get; set; }

        // Navigation property (optional)
        [ForeignKey("PatientId")]
        public Patient? Patient { get; set; }
    }
}
