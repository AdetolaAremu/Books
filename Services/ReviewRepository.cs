using bookreview.DataStore;
using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public class ReviewRepository : IReviewRepository
  {
    private ApplicationDbContext _reviewContext;

    public ReviewRepository(ApplicationDbContext reviewContext)
    {
      _reviewContext = reviewContext;
    }

    public ICollection<Review> GetAllReviews()
    {
      return _reviewContext.Reviews.ToList();
    }

    public Review GetReview(int reviewId)
    {
      return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
    }

    public ICollection<Review> GetReviewsOfABook(int bookId)
    {
      return _reviewContext.Books.Where(b => b.Id == bookId).SelectMany(r => r.Reviews).ToList();
    }

    public Book GetBookOfAReview(int reviewId)
    {
      return _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(c => c.Books).FirstOrDefault();
    }

    public bool ReviewExists(int reviewId)
    {
      return _reviewContext.Reviews.Any(r => r.Id == reviewId);
    }

    public bool CreateReview(Review review)
    {
      _reviewContext.Add(review);
      return SaveReview();
    }

    public bool UpdateReview(ReviewDTO reviewDTO)
    {
      var review = _reviewContext.Reviews.Where(r => r.Id == reviewDTO.Id).First();
      
      review.Headline = reviewDTO.Headline;
      review.Body = reviewDTO.Body;
      review.Rating = reviewDTO.Rating;
      
      return SaveReview();
    }

    public bool DeleteReview(Review review)
    {
      _reviewContext.Remove(review);
      return SaveReview();
    }

    public bool SaveReview()
    {
      var checkSaved = _reviewContext.SaveChanges();
      return checkSaved >= 0 ? true : false;
    }
  }
}