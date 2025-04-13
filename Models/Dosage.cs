using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("dosages", Schema = "public")]
    public class Dosage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("medication_id")]
        public int MedicationId { get; set; }

        [Required]
        [Column("dosage_amount")]
        public string DosageAmount { get; set; }

        [Required]
        [Column("dosage_frequency")]
        public string DosageFrequency { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("MedicationId")]
        public Medication Medication { get; set; }
    }
}

