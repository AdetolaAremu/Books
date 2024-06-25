using bookreview.Models;

namespace bookreview.Services
{
  public interface IReviewerRepository
  {
    ICollection<Reviewer> Reviewers();
    Reviewer GetReviewer(int reviewerId);
    ICollection<Review> GetReviewsByReviewer(int reviewerId);
    Reviewer GetReviewerOfAReview(int reviewId);
    bool ReviewerExists(int reviewerId);
  }
}