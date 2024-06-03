using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookreview.Models
{
  public class Author
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3,ErrorMessage = "First Name must be at least 3 characters and cannot be more than 150 characters")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3,ErrorMessage = "Last  Name must be at least 3 characters and cannot be more than 150 characters")]
    public string LastName { get; set; }
     
    public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    public virtual Country Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}