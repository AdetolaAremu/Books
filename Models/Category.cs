using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookreview.Models
{
  public class Category
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters long and cannot be more than 100 characters")]
    public string Name { get; set; }

    public virtual ICollection<BookCategory> BookCategories { get; set; } 
     public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}