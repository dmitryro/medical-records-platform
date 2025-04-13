using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("medication_interaction_primaries", Schema = "public")]
    public class MedicationInteractionPrimary
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("medication_id")]
        public int MedicationId { get; set; }

        [Required]
        [Column("interaction_name")]
        public string InteractionName { get; set; }

        [Required]
        [Column("interaction_class")]
        public string InteractionClass { get; set; }

        [Column("description")]
        public string Description { get; set; }

        // Foreign key property
        [ForeignKey("MedicationId")]
        public Medication Medication { get; set; }
    }
}

