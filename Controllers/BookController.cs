using bookreview.DTO;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [ApiController]
  [Route("/api/books")]
  public class BookController : ControllerBase
  {
    private IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
      _bookRepository = bookRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllBooks()
    {
      var books = _bookRepository.GetAllBooks().ToList();

      if (!ModelState.IsValid) return BadRequest();

      var bookDTO = new List<BookDTO>();

      foreach (var book in books)
      {
        bookDTO.Add(new BookDTO {
          Id = book.Id,
          Title = book.Title,
          Isbn = book.Isbn,
          DatePublished = book.DatePublished
        });
      }

      return Ok(bookDTO);
    }

    [HttpGet("{bookId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetABook(int bookId)
    {
      if (!_bookRepository.BookExists(bookId)) return NotFound();

      var book = _bookRepository.GetBook(bookId);

      if (!ModelState.IsValid) return BadRequest();

      var bookDTO = new BookDTO(){
        Id = book.Id,
        Title = book.Title,
        Isbn = book.Isbn,
        DatePublished = book.DatePublished
      };

      return Ok(bookDTO);
    }

    [HttpGet("{bookId}/ratings")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetBookRatings(int bookId)
    {
      if (!_bookRepository.BookExists(bookId)) return NotFound();

      var review = _bookRepository.GetBookRating(bookId);

      if (!ModelState.IsValid) return BadRequest();

      return Ok(review);
    }
  }
}