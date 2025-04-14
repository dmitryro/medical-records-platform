using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("master_patient_index", Schema = "public")]
    public class MasterPatientIndex
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("patient_id")]
        public int? PatientId { get; set; } // Changed to int?

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("gender")]
        public string? Gender { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("contact_number")]
        public string? ContactNumber { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("social_security_number")]
        public string? SocialSecurityNumber { get; set; }

        [Column("match_score")]
        public float? MatchScore { get; set; }

        [Column("match_date")]
        public DateTime? MatchDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

