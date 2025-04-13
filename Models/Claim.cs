using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("claims", Schema = "public")]
    public class Claim
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [Required]
        [Column("insurance_id")]
        public int InsuranceId { get; set; }

        [ForeignKey("InsuranceId")]
        public Insurance Insurance { get; set; }

        [Required]
        [Column("date_of_service")]
        public DateTime DateOfService { get; set; }

        [Required]
        [Column("amount_billed")]
        public float AmountBilled { get; set; }

        [Required]
        [Column("amount_covered")]
        public float AmountCovered { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }
    }
}

