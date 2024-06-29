using bookreview.DTO;
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

    bool CreateReviewer(Reviewer reviewer);
    bool UpdateReviewer(ReviewerDTO reviewerDTO);
    bool DeleteReviewer(Reviewer reviewer);
    bool SaveReviewer();
  }
}