using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("medication_interactions", Schema = "public")]
    public class MedicationInteraction
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

        // Foreign key properties
        [ForeignKey("PrimaryMedicationId")]
        public Medication PrimaryMedication { get; set; }

        [ForeignKey("SecondaryMedicationId")]
        public Medication SecondaryMedication { get; set; }
    }
}

