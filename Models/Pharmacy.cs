using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("pharmacies", Schema = "public")]
    public class Pharmacy
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("contact_number")]
        public string? ContactNumber { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("pharmacy_type")]
        public string? PharmacyType { get; set; }

        // Navigation properties
        // Assuming Pharmacy is related to other entities like Prescriptions
        // public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
