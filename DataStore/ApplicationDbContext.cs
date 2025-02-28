using bookreview.DataStore.Seeders;
using bookreview.Models;
using Microsoft.EntityFrameworkCore;

namespace bookreview.DataStore
{
  public class ApplicationDbContext: DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
      // SeedDatabase();
    }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<BookCategory> BookCategories { get; set; }
    public virtual DbSet<BookAuthor> BookAuthors { get; set; }
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Reviewer> Reviewers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // We set relationships
      modelBuilder.Entity<BookCategory>()
        .HasKey(bc => new { bc.BookId, bc.CategoryId });

      modelBuilder.Entity<BookCategory>()
        .HasOne(b => b.Book)
        .WithMany(c => c.BookCategories)
        .HasForeignKey(b => b.BookId);

      modelBuilder.Entity<BookCategory>()
        .HasOne(c => c.Category)
        .WithMany(bc => bc.BookCategories)
        .HasForeignKey(c => c.CategoryId);

      modelBuilder.Entity<BookAuthor>()
        .HasKey(ba => new { ba.AuthorId, ba.BookId });

      modelBuilder.Entity<BookAuthor>()
        .HasOne(a => a.Author)
        .WithMany(ba => ba.BookAuthors)
        .HasForeignKey(a => a.AuthorId);

      modelBuilder.Entity<BookAuthor>()
        .HasOne(b => b.Book)
        .WithMany(ba => ba.BookAuthors)
        .HasForeignKey(a => a.BookId);
    }

    // private void SeedDatabase()
    // {
    //   this.SeedDataContext();
    // }
  }
}