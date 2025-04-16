using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalAPI.Models
{
    [Table("addresses", Schema = "public")]
    public class Address
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("address1")]
        public string Address1 { get; set; }

        [Required]
        [Column("address2")]
        public string Address2 { get; set; }

        [Column("floor")]
        public string Floor { get; set; }

        [Column("room")]
        public string Room { get; set; }   

        [Column("apartment")]
        public string Apartment { get; set; }

        [Required]
        [Column("address_type")]
        public string AddressType { get; set; }     

        [Required]
        [Column("city")]
        public string City { get; set; }

        [Required]
        [Column("state_province_id")]
        public int StateProvinceId { get; set; }

        [Column("postal_code")]
        public string PostalCode { get; set; }

        [Column("county")]
        public string County{ get; set; }

        [Column("region")]
        public string Region{ get; set; }

        [Required]
        [Column("country")]
        public string Country { get; set; }

        [ForeignKey("StateProvinceId")]
        public virtual StateProvince StateProvince { get; set; }
    }
}

