namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int comment_id { get; set; }

        [StringLength(500)]
        public string context { get; set; }

        public int? user_id { get; set; }

        public int? book_id { get; set; }

        public DateTime? date { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
