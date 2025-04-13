using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("refills", Schema = "public")]
    public class Refill
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("prescription_id")]
        public int PrescriptionId { get; set; }

        [Required]
        [Column("date_requested")]
        public DateTime DateRequested { get; set; }

        [Column("date_fulfilled")]
        public DateTime? DateFulfilled { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }

        // Navigation property
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
    }
}

