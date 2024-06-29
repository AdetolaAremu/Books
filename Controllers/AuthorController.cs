using bookreview.DTO;
using bookreview.Helpers;
using bookreview.Models;
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
          LastName = author.LastName,
          CountryId = author.CountryId
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
        LastName = author.LastName,
        CountryId = author.CountryId
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
          LastName = author.LastName,
          CountryId = author.CountryId
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthorDTO))]
    public IActionResult CreateAuthor([FromBody] AuthorDTO authorDTO)
    {
      if (authorDTO == null) return ResponseHelper.ErrorResponseHelper("Invalid request, cannot be empty", null, 422);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var createAuthor = _authorRepository.CreateAuthor(new Author{
        FirstName = authorDTO.FirstName,
        LastName = authorDTO.LastName,
        CountryId = authorDTO.CountryId
      });

      if (!createAuthor) return ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {authorDTO}", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<object>("Author created successfully", authorDTO, 201);
    }

    [HttpPut("{authorId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthorDTO))]
    public IActionResult UpdateAuthor(int authorId, [FromBody] AuthorDTO authorDTO)
    {
      if (authorDTO == null) return ResponseHelper.ErrorResponseHelper("Invalid request, cannot be empty", null, 422);

      if (!_authorRepository.AuthorExists(authorId)) return ResponseHelper.ErrorResponseHelper("Author not found", null, 404);

      if (authorId != authorDTO.Id) return ResponseHelper.ErrorResponseHelper("ID mismatch");

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var updateAuthor = _authorRepository.UpdateAuthor(authorDTO);

      if (!updateAuthor) return ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {authorDTO}", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<object>("Author created successfully", authorDTO, 201);
    }

    [HttpDelete("{authorId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteAuthor(int authorId)
    {
      if (!_authorRepository.AuthorExists(authorId)) return ResponseHelper.ErrorResponseHelper("Author not found", null, 404);

      if (_authorRepository.GetBookByAuthor(authorId).Any()) 
        return ResponseHelper.ErrorResponseHelper("Author has books, cannot be deleted", null, 400);

      var author = _authorRepository.GetAuthor(authorId);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var deleteAuthor = _authorRepository.DeleteAuthor(author);
      
      if (!deleteAuthor) ResponseHelper.ErrorResponseHelper("Something went wrong while deleting your record, please retry", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<string>("Author deleted successfully", null, 200);
    }
  }
}