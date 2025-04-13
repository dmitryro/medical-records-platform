using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("nurses", Schema = "public")]
    public class Nurse
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("credentials")]
        public string Credentials { get; set; }

        [Column("specialization")]
        public string Specialization { get; set; }

        [Column("assigned_doctor_id")]
        public int? AssignedDoctorId { get; set; }

        [Column("shift_schedule")]
        public string ShiftSchedule { get; set; }

        [Required]
        [Column("contact_info")]
        public string ContactInfo { get; set; }

        [Required]
        [Column("employment_status")]
        public string EmploymentStatus { get; set; }

        // Foreign key property
        [ForeignKey("AssignedDoctorId")]
        public Doctor AssignedDoctor { get; set; }
    }
}

