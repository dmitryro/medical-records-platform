using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("social_determinants", Schema = "public")]
    public class SocialDeterminant
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Required]
        [Column("factor_type")]
        public string FactorType { get; set; }

        [Column("details")]
        public string? Details { get; set; }

        [Column("recorded_by")]
        public int? RecordedBy { get; set; }

        [Required]
        [Column("recorded_at")]
        public DateTime RecordedAt { get; set; }

        // Navigation property to Patient
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        // Navigation property to User who recorded the entry
        [ForeignKey("RecordedBy")]
        public virtual User? Recorder { get; set; }
    }
}

