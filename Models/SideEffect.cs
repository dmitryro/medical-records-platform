using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("side_effects", Schema = "public")]
    public class SideEffect
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("medication_id")]
        public int MedicationId { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("severity")]
        public string Severity { get; set; }

        [Column("onset")]
        public string? Onset { get; set; }

        [Column("duration")]
        public string? Duration { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        [ForeignKey("MedicationId")]
        public virtual Medication Medication { get; set; }
    }
}

