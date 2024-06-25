using bookreview.DTO;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [Route("api/reviewers")]
  [ApiController]
  public class ReviewerController : ControllerBase
  {
    private IReviewerRepository _reviewerRepository;

    public ReviewerController(IReviewerRepository reviewerRepository)
    {
      _reviewerRepository = reviewerRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllReviewers()
    {
      var reviewers = _reviewerRepository.GetReviewers().ToList();

      if (!ModelState.IsValid) return BadRequest();

      var reviewerDto = new List<ReviewerDTO>();

      foreach (var reviewer in reviewers)
      {
        reviewerDto.Add(new ReviewerDTO{
          Id = reviewer.Id,
          FirstName = reviewer.FirstName,
          LastName = reviewer.LastName
        });
      }

      return Ok(reviewerDto);
    }

    [HttpGet("{reviewerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReviewer(int reviewerId)
    {
      var checkIfReviewerExists = _reviewerRepository.ReviewerExists(reviewerId);

      if (!checkIfReviewerExists) return NotFound();

      var reviewer = _reviewerRepository.GetReviewer(reviewerId);

      if (!ModelState.IsValid) return BadRequest();

      var reviewerDTO = new ReviewerDTO{
        Id = reviewer.Id,
        FirstName = reviewer.FirstName,
        LastName = reviewer.LastName
      };

      return Ok(reviewerDTO);
    }

    [HttpGet("{reviewerId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReviewsByReviewer(int reviewerId)
    {
      var checkIfReviewerExists = _reviewerRepository.ReviewerExists(reviewerId);

       if (!checkIfReviewerExists) return NotFound();

      var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

      if (!ModelState.IsValid) return BadRequest();

      var reviewDTO = new List<ReviewDTO>();

      foreach (var review in reviews)
      {
        reviewDTO.Add(new ReviewDTO{
          Id = review.Id,
          Body = review.Body,
          Headline = review.Headline,
          Rating = review.Rating
        });
      }

      return Ok(reviews);
    }

    [HttpGet("details/{reviewerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetReviewerOfAReview(int reviewerId)
    {
      var checkIfReviewerExists = _reviewerRepository.ReviewerExists(reviewerId);

       if (!checkIfReviewerExists) return NotFound();

      var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewerId);

      if (!ModelState.IsValid) return BadRequest();

      var reviewerDTO = new ReviewerDTO(){
        Id = reviewer.Id,
        FirstName = reviewer.FirstName,
        LastName = reviewer.LastName
      };

      return Ok(reviewerDTO);
    }
  }
}