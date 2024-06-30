using bookreview.DTO;
using bookreview.Helpers;
using bookreview.Models;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [ApiController]
  [Route("/api/books")]
  public class BookController : ControllerBase
  {
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BookController(IBookRepository bookRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
    {
      _bookRepository = bookRepository;
      _authorRepository = authorRepository;
      _categoryRepository = categoryRepository;
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BookDTO))]
    public IActionResult CreateBook([FromQuery] List<int> authId, [FromQuery] List<int> catId, [FromBody] BookDTO book)
    {
      var statusCode = ValidateBook(book, authId, catId, "create");

      if (statusCode != null) return statusCode;

      var bookCreate = new Book(){
        Isbn = book.Isbn,
        Title = book.Title,
        DatePublished = book.DatePublished
      };

      var createBook = _bookRepository.CreateBook(bookCreate, authId, catId);

      if (!createBook) return ResponseHelper.ErrorResponseHelper("Something went wrong while saving your record", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<object>("Book created successfully", book, 201); 
    }

    [HttpPut("{bookId}")]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDTO))]
    public IActionResult UpdateBook(int bookId, [FromQuery] List<int> authId, [FromQuery] List<int> catId, [FromBody] BookDTO book)
    {
      if (bookId != book.Id) return ResponseHelper.ErrorResponseHelper("ID mismatch");

      if (!_bookRepository.BookExists(bookId)) return ResponseHelper.ErrorResponseHelper("Book not found", null, 404);

      var statusCode = ValidateBook(book, authId, catId, "update");

      if (statusCode != null) return statusCode;

      var createBook = _bookRepository.UpdateBook(book, authId, catId);

      if (!createBook) return ResponseHelper.ErrorResponseHelper("Something went wrong while saving your record", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<object>("Book updated successfully", book, 200); 
    }

    [HttpDelete("{bookId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteBook(int bookId)
    {
      if (!_bookRepository.BookExists(bookId)) return ResponseHelper.ErrorResponseHelper("Book not found", null, 404);

      var book = _bookRepository.GetBook(bookId);

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var deleteAuthor = _bookRepository.DeleteBook(book);
      
      if (!deleteAuthor) ResponseHelper.ErrorResponseHelper("Something went wrong while deleting your record, please retry", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<string>("Book deleted successfully", null, 200);
    }

    private IActionResult ValidateBook(BookDTO book, List<int> authorsId, List<int> categoriesId, string updateType)
    {
      if (updateType != "update")
      {
        if (book == null || authorsId.Count <= 0 || categoriesId.Count <= 0)
        {
          return ResponseHelper.ErrorResponseHelper("Book, Authors or categories cannot be null", null, 422);
        }
      }

      if (updateType == "update")
      {
        if (book == null)
        {
          return ResponseHelper.ErrorResponseHelper("Book cannot be null", null, 422);
        }
      }

      if (_bookRepository.isDuplicateISBN(book.Isbn, book.Id)) return
        ResponseHelper.ErrorResponseHelper("Duplicate ISBN discovered");

      if (authorsId.Count() != _authorRepository.GetCountOfAuthorsPassed(authorsId))
        return ResponseHelper.ErrorResponseHelper("At least one author not found");

      if (categoriesId.Count() != _categoryRepository.GetCountOfCategoriesPassed(authorsId))
        return ResponseHelper.ErrorResponseHelper("At least one category not found");

      if (!ModelState.IsValid) return ResponseHelper.ErrorResponseHelper("Invalid request", ModelState);

      return null;
    }
  }
}