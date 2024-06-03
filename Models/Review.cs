using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookreview.Models
{
  public class Review
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Headline must be at least 3 characters long and cannot be more than 100 characters")]
    public string Headline { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10, ErrorMessage = "Headline must be at least 10 characters long and cannot be more than 300 characters")]
    public string Body { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must  be between 1 and 5 stars")]
    public int Rating { get; set; }
    public virtual Reviewer Reviewers { get; set; }
    public virtual Book Books { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  } 
}