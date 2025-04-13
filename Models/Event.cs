using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("events", Schema = "public")]
    public class Event
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("patient_id")]
        public int PatientId { get; set; }

        [Required]
        [Column("event_type")]
        public string EventType { get; set; }

        [Required]
        [Column("event_date")]
        public DateTime EventDate { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        // Navigation property
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}

