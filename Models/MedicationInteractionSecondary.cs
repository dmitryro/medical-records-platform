using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("medication_interaction_secondaries", Schema = "public")]
    public class MedicationInteractionSecondary
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("primary_medication_id")]
        public int PrimaryMedicationId { get; set; }

        [Required]
        [Column("secondary_medication_id")]
        public int SecondaryMedicationId { get; set; }

        [Required]
        [Column("severity")]
        public string Severity { get; set; }

        [Column("description")]
        public string Description { get; set; }

        // Foreign key properties
        [ForeignKey("PrimaryMedicationId")]
        public MedicationInteractionPrimary PrimaryMedicationInteraction { get; set; }

        [ForeignKey("SecondaryMedicationId")]
        public Medication SecondaryMedication { get; set; }
    }
}

