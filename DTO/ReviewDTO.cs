using System.ComponentModel.DataAnnotations;

namespace bookreview.DTO
{
  public class ReviewDTO
  {
    public int Id { get; set; }
    public string Headline { get; set; }
    public string Body { get; set; }
    public int Rating { get; set; }
  }

  public class CreateReviewDTO
  {
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Headline must be at least 3 characters long and cannot be more than 100 characters")]
    public string Headline { get; set; }

    [Required]
    [StringLength(300, MinimumLength = 10, ErrorMessage = "Headline must be at least 10 characters long and cannot be more than 300 characters")]
    public string Body { get; set; }

    [Required]
    [Range(1, 5, ErrorMessage = "Rating must  be between 1 and 5 stars")]
    public int Rating { get; set; }

    [Required]
    public int ReviewerId { get; set; }

    [Required]
    public int BookId { get; set; }
  }
}