using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("partners", Schema = "public")]
    public class Partner
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("contact_info")]
        public string ContactInfo { get; set; }

        [Required]
        [Column("partnership_type")]
        public string PartnershipType { get; set; }
    }
}
