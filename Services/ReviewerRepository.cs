using bookreview.DataStore;
using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public class ReviewerRepository : IReviewerRepository
  {
    private ApplicationDbContext _reviewerContext, _reviewContext;
    // private ApplicationDbContext _reviewContext;

    public ReviewerRepository(ApplicationDbContext reviewerContext, ApplicationDbContext reviewContext)
    {
      _reviewerContext = reviewerContext;
      _reviewContext = reviewContext;
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
      // return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).SelectMany(r => r.Reviews).ToList();
      return _reviewerContext.Reviews.Where(r => r.Reviewers.Id == reviewerId).ToList();
    }

    public Reviewer GetReviewerOfAReview(int reviewId)
    {
      return _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(r => r.Reviewers).FirstOrDefault();
      
      // or (n + 1)
      // var reviewerId = _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(rr => rr.Reviewers.Id).FirstOrDefault();
      // return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId);
    }

    public bool ReviewerExists(int reviewerId)
    {
      return _reviewerContext.Reviewers.Any(r => r.Id == reviewerId);
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
      _reviewerContext.Reviewers.Add(reviewer);
      return SaveReviewer();
    }

    public bool UpdateReviewer(ReviewerDTO reviewer)
    {
      var getReviewer = _reviewerContext.Reviewers.Where(rr => rr.Id == reviewer.Id).First();

      getReviewer.FirstName = reviewer.FirstName;
      getReviewer.LastName = reviewer.LastName;

      return SaveReviewer();
    }

    public bool DeleteReviewer(Reviewer reviewer)
    {
      // get the reviewer first
      var reviews = _reviewContext.Reviews.Where(r => r.ReviewerId == reviewer.Id).ToList();
      
      if (reviews.Any()) _reviewContext.RemoveRange(reviews);

      _reviewerContext.Remove(reviewer);

      return SaveReviewer();
    }

    public bool SaveReviewer()
    {
      var reviewer = _reviewerContext.SaveChanges();
      return reviewer >= 0 ? true : false;
    }
  }
}