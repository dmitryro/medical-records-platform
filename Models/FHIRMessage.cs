using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("fhir_messages", Schema = "public")]
    public class FHIRMessage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("message_type")]
        public string MessageType { get; set; }

        [Required]
        [Column("message_content")]
        public string MessageContent { get; set; }

        [Required]
        [Column("received_date")]
        public DateTime ReceivedDate { get; set; }

        [Column("sent_date")]
        public DateTime? SentDate { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }
    }
}

