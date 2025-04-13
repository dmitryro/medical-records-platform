using System.ComponentModel.DataAnnotations;

namespace MedicalAPI.Data
{
    public class RegistrationDto
    {
        [Required]
        public string First { get; set; } = null!; // Make non-nullable and required
        [Required]
        public string Last { get; set; } = null!;  // Make non-nullable and required
        [Required]
        public string Username { get; set; } = null!; // Make non-nullable and required
        [Required]
        public string Email { get; set; } = null!;    // Make non-nullable and required
        [Required]
        public string Password { get; set; } = null!; // Make non-nullable and required
        [Required]
        public string Phone { get; set; } = null!;    // Make non-nullable and required
        [Required]
        public uint RoleId { get; set; }
    }
}
