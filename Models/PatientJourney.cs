using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("patient_journeys", Schema = "public")]
    public class PatientJourney
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Required]
        [Column("encounter_id")]
        public int EncounterId { get; set; }

        [Required]
        [Column("diagnosis_id")]
        public int DiagnosisId { get; set; }

        [Column("prescription_id")]
        public int? PrescriptionId { get; set; }

        [Column("vitals_id")]
        public int? VitalsId { get; set; }

        [Required]
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        // Navigation properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("EncounterId")]
        public virtual Encounter Encounter { get; set; }

        [ForeignKey("DiagnosisId")]
        public virtual Diagnosis Diagnosis { get; set; }

        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }

        [ForeignKey("VitalsId")]
        public virtual Vitals Vitals { get; set; }
    }
}

