using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("medical_codes", Schema = "public")]
    public class MedicalCode
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("code_type")]
        public string CodeType { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
