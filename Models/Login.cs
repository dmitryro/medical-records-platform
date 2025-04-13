using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("logins", Schema = "public")]
    public class Login
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Required]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Column("login_time")]
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    }
}

