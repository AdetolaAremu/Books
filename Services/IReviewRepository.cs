using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public interface IReviewRepository
  {
    ICollection<Review> GetAllReviews();
    Review GetReview(int reviewId);
    ICollection<Review> GetReviewsOfABook(int bookId);
    Book GetBookOfAReview(int reviewId);
    bool ReviewExists(int reviewId);

    bool CreateReview(Review review);
    bool UpdateReview(ReviewDTO reviewDTO);
    bool DeleteReview(Review review);
    bool SaveReview();
  }
}