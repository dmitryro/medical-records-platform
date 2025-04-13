using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("immunizations", Schema = "public")]
    public class Immunization
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Required]
        [Column("vaccine_name")]
        public string VaccineName { get; set; }

        [Required]
        [Column("administration_date")]
        public DateTime AdministrationDate { get; set; }

        [Column("administered_by")]
        public int? AdministeredBy { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        // Foreign Key to Patient (assuming a Patient class exists)
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        // Foreign Key to AdministeredBy (assuming a User class exists)
        [ForeignKey("AdministeredBy")]
        public virtual User AdministeredByUser { get; set; }
    }
}

