using bookreview.Models;

namespace bookreview.Services
{
  public interface IReviewerRepository
  {
    ICollection<Reviewer> GetReviewers();
    Reviewer GetReviewer(int reviewerId);
    ICollection<Review> GetReviewsByReviewer(int reviewerId);
    Reviewer GetReviewerOfAReview(int reviewId);
    bool ReviewerExists(int reviewerId);
  }
}