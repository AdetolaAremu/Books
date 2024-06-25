using bookreview.DataStore;
using bookreview.Models;

namespace bookreview.Services
{
  public class ReviewerRepository : IReviewerRepository
  {
    private ApplicationDbContext _reviewerContext;

    public ReviewerRepository(ApplicationDbContext reviewerContext)
    {
      _reviewerContext = reviewerContext;
    }

    public ICollection<Reviewer> GetReviewers()
    {
      return _reviewerContext.Reviewers.ToList();
    }

    public Reviewer GetReviewer(int reviewerId)
    {
      return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
    }

    public ICollection<Review> GetReviewsByReviewer(int reviewerId)
    {
      return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).SelectMany(r => r.Reviews).ToList();
    }

    public Reviewer GetReviewerOfAReview(int reviewId)
    {
      return _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewers).FirstOrDefault();
    }

    public bool ReviewerExists(int reviewerId)
    {
      return _reviewerContext.Reviewers.Any(r => r.Id == reviewerId);
    }
  }
}