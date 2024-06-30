using System.ComponentModel.DataAnnotations;

namespace bookreview.Models
{
  public class BookAuthor
  {
    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; }
    
    public int AuthorId { get; set; }
    public Author Author { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}