namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class bookstoreDB : DbContext
    {
        public bookstoreDB()
            : base("name=bookstoreDB")
        {
        }

        public virtual DbSet<Authority> Authorities { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ContactInfo> ContactInfoes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.b_name)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Books)
                .Map(m => m.ToTable("BookTag").MapLeftKey("book_id").MapRightKey("tag_id"));
        }
    }
}
