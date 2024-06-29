using bookreview.DTO;
using bookreview.Helpers;
using bookreview.Models;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [ApiController]
  [Route("/api/reviews")]
  public class ReviewController : ControllerBase
  {
    private IReviewRepository _reviewRepository;
    private IBookRepository _bookRepository;

    public ReviewController(IReviewRepository reviewController, IBookRepository bookRepository)
    {
      _reviewRepository = reviewController;
      _bookRepository = bookRepository;
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

      return ResponseHelper.SuccessResponseHelper("Reviews retrieved successfully", reviewDTO);
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

      return ResponseHelper.SuccessResponseHelper("Reviews retrieved successfully", reviewDTO);
    }

    [HttpGet("{bookId}/book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReviewsOfABook(int bookId)
    {
      // Todo: check if book exists
      if (!_bookRepository.BookExists(bookId)) return NotFound();

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

      return ResponseHelper.SuccessResponseHelper("Reviews retrieved successfully", reviewDTO);
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

      return ResponseHelper.SuccessResponseHelper("Book retrieved successfully", bookDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewDTO))]
    public IActionResult CreateReview([FromBody] CreateReviewDTO reviewDTO)
    {
      if (reviewDTO == null) return ResponseHelper.ErrorResponseHelper("Invalid request body", new { ModelState }, 422);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var review = _reviewRepository.CreateReview(new Review{
        Headline = reviewDTO.Headline,
        Body = reviewDTO.Body,
        Rating = reviewDTO.Rating,
        ReviewerId = reviewDTO.ReviewerId,
        BookId = reviewDTO.BookId,
      });

      if (!review) return ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {reviewDTO}", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<object>("Review created successfully", reviewDTO, 201);
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewDTO))]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO reviewDto)
    {
      if (reviewDto == null) return ResponseHelper.ErrorResponseHelper("Invalid request body", new { ModelState }, 422);

      if(!_reviewRepository.ReviewExists(reviewId)) return ResponseHelper.ErrorResponseHelper("Review does not exist", null, 404);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      if (reviewId != reviewDto.Id) return ResponseHelper.ErrorResponseHelper("ID mismatch");

      var review = _reviewRepository.UpdateReview(reviewDto);

      if (!review) return ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {reviewDto}", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<object>("Review updated successfully", reviewDto);
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteReview(int reviewId)
    {
      if (!_reviewRepository.ReviewExists(reviewId)) return ResponseHelper.ErrorResponseHelper("Review does not exist", null, 404);

      var review = _reviewRepository.GetReview(reviewId);

      if (!ModelState.IsValid) BadRequest(ModelState);

      var deleteReview = _reviewRepository.DeleteReview(review);

      if (!deleteReview) ResponseHelper.ErrorResponseHelper($"Something went wrong while deleting your record, please retry", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<string>("Review deleted successfully", null, 200);
    }
  }
}