using bookreview.DTO;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [ApiController]
  [Route("/api/reviews")]
  public class ReviewController : ControllerBase
  {
    private IReviewRepository _reviewRepository;

    public ReviewController(IReviewRepository reviewController)
    {
      _reviewRepository = reviewController;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReviews()
    {
      var reviews = _reviewRepository.GetAllReviews().ToList();

      if (!ModelState.IsValid) return BadRequest();

      var reviewDTO = new List<ReviewDTO>();

      foreach (var review in reviews)
      {
        reviewDTO.Add(new ReviewDTO{
          Id = review.Id,
          Headline = review.Headline,
          Body = review.Body,
          Rating = review.Rating
        });
      }

      return Ok(reviewDTO);
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReview(int reviewId)
    {
      if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();

      var review = _reviewRepository.GetReview(reviewId);

      if (!ModelState.IsValid) return BadRequest();

      var reviewDTO = new ReviewDTO(){
        Id = review.Id,
        Headline = review.Headline,
        Body = review.Body,
        Rating = review.Rating
      };

      return Ok(reviewDTO);
    }

    [HttpGet("{bookId}/book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReviewsOfABook(int bookId)
    {
      // Todo: check if book exists

      var getBook = _reviewRepository.GetReviewsOfABook(bookId);

      if (!ModelState.IsValid) return BadRequest();

      var reviewDTO = new List<ReviewDTO>();

      foreach (var review in getBook)
      {
        reviewDTO.Add(new ReviewDTO{
          Id = review.Id,
          Headline = review.Headline,
          Body = review.Body,
          Rating = review.Rating
        });
      }

      return Ok(reviewDTO);
    }

    [HttpGet("bookdetails/{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetBookOfAReview(int reviewId)
    {
      if (!_reviewRepository.ReviewExists(reviewId)) return NotFound();

      var book = _reviewRepository.GetBookOfAReview(reviewId);

      if (!ModelState.IsValid) return BadRequest();

      var bookDto = new BookDTO(){
        Id = book.Id,
        Isbn = book.Isbn,
        Title = book.Title,
        DatePublished = book.DatePublished
      };

      return Ok(bookDto);
    }
  }
}