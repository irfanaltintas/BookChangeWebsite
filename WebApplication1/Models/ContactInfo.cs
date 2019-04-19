namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContactInfo")]
    public partial class ContactInfo
    {
        [Key]
        public int contact_id { get; set; }

        [StringLength(250)]
        public string contact_info { get; set; }

        public int? user_id { get; set; }

        public int? phonenumber { get; set; }

        [StringLength(150)]
        public string namesurname { get; set; }

        [StringLength(150)]
        public string email { get; set; }

        public int? book_id { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
