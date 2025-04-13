using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("x12_edi_messages", Schema = "public")]
    public class X12EDIMessage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("transaction_set_id")]
        public string TransactionSetId { get; set; }

        [Required]
        [Column("transaction_set_control_number")]
        public string TransactionSetControlNumber { get; set; }

        [Required]
        [Column("interchange_control_number")]
        public string InterchangeControlNumber { get; set; }

        [Required]
        [Column("sender_id")]
        public string SenderId { get; set; }

        [Required]
        [Column("receiver_id")]
        public string ReceiverId { get; set; }

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

