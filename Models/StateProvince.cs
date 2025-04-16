using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("states_provinces", Schema = "public")]
    public class StateProvince {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("mame")]
        public string Name { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Column("country")]
        public string Country { get; set; }
    }
}
