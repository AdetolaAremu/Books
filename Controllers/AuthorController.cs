using bookreview.DTO;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [Route("/api/authors")]
  [ApiController]
  public class AuthorController : ControllerBase
  {
    private IAuthorRepository _authorRepository;
    private IBookRepository _bookRepository;

    public AuthorController(IAuthorRepository authorRepository, IBookRepository bookRepository)
    {
      _authorRepository = authorRepository;
      _bookRepository = bookRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllAuthors()
    {
      var authors = _authorRepository.GetAllAuthors().ToList();

      if (!ModelState.IsValid) return BadRequest();

      var authorDto = new List<AuthorDTO>();

      foreach (var author in authors)
      {
        authorDto.Add(new AuthorDTO{
          Id = author.Id,
          FirstName = author.FirstName,
          LastName = author.LastName
        });
      }

      return Ok(authorDto);
    }

    [HttpGet("{authorId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAuthor(int authorId)
    {
      if (!_authorRepository.AuthorExists(authorId)) return NotFound();

      var author = _authorRepository.GetAuthor(authorId);

      if (!ModelState.IsValid) return BadRequest();

      var authorDTO = new AuthorDTO(){
        Id = author.Id,
        FirstName = author.FirstName,
        LastName = author.LastName
      };

      return Ok(authorDTO);
    }

    [HttpGet("{bookId}/book")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAuthorsOfABook(int bookId)
    {
      if (!_bookRepository.BookExists(bookId)) return NotFound();

      var authors = _authorRepository.GetAuthorsOfABook(bookId).ToList();

      if (!ModelState.IsValid) return BadRequest();

       var authorDto = new List<AuthorDTO>();

      foreach (var author in authors)
      {
        authorDto.Add(new AuthorDTO{
          Id = author.Id,
          FirstName = author.FirstName,
          LastName = author.LastName
        });
      }

      return Ok(authorDto);
    }

    [HttpGet("books/{authorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetBookByAuthor(int authorId)
    {
      var authors = _authorRepository.GetBookByAuthor(authorId).ToList();

      if (!ModelState.IsValid) return BadRequest();

      var bookDTO = new List<BookDTO>();

      foreach (var author in authors)
      {
        bookDTO.Add(new BookDTO{
          Id = author.Id,
          Title = author.Title,
          Isbn = author.Isbn,
          DatePublished = author.DatePublished
        });
      }

      return Ok(bookDTO);
    }
  }
}