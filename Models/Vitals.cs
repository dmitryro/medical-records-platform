using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("vitals", Schema = "public")]
    public class Vitals
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("weight")]
        public float? Weight { get; set; }

        [Column("height")]
        public float? Height { get; set; }

        [Column("blood_pressure_systolic")]
        public int? BloodPressureSystolic { get; set; }

        [Column("blood_pressure_diastolic")]
        public int? BloodPressureDiastolic { get; set; }

        [Column("temperature")]
        public float? Temperature { get; set; }

        [Column("heart_rate")]
        public int? HeartRate { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (optional)
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}

