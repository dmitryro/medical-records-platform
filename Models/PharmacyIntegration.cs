using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("pharmacy_integrations", Schema = "public")]
    public class PharmacyIntegration
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("pharmacy_id")]
        public int PharmacyId { get; set; }

        [Required]
        [Column("prescription_id")]
        public int PrescriptionId { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }

        [Column("fulfillment_date")]
        public DateTime? FulfillmentDate { get; set; }

        // Navigation properties
        [ForeignKey("PharmacyId")]
        public virtual Pharmacy Pharmacy { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
    }
}

