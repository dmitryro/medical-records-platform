using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("users", Schema = "public")]
    public class User
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Required]
        [Column("first")]
        public string First { get; set; }

        [Required]
        [Column("last")]
        public string Last { get; set; }

        [Required]
        [Column("username")]   
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")] 
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("phone")] 
        public string Phone { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")] 
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key property
        [Column("role_id")]
        public uint RoleId { get; set; }

        // Navigation property
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
