namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            Comments = new HashSet<Comment>();
            ContactInfoes = new HashSet<ContactInfo>();
            Tags = new HashSet<Tag>();
        }

        [Key]
        public int book_id { get; set; }

        [StringLength(100)]
        public string b_name { get; set; }

        [StringLength(500)]
        public string photo { get; set; }

        [StringLength(50)]
        public string writer { get; set; }

        [StringLength(50)]
        public string p_house { get; set; }

        [StringLength(500)]
        public string aboutbook { get; set; }

        public int? category_id { get; set; }

        public int? user_id { get; set; }

        public int? request { get; set; }

        public virtual Category Category { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContactInfo> ContactInfoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
