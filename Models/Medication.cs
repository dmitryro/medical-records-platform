using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("medications", Schema = "public")]
    public class Medication
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("brand_name")]
        public string BrandName { get; set; }

        [Column("generic_name")]
        public string GenericName { get; set; }

        [Required]
        [Column("medication_class")]
        public string MedicationClass { get; set; } // e.g., Antibiotic, Analgesic
    }
}

