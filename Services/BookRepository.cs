using bookreview.DataStore;
using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public class BookRepository : IBookRepository
  {
    private ApplicationDbContext _applicationDBContext;

    public BookRepository(ApplicationDbContext applicationDbContext)
    {
      _applicationDBContext = applicationDbContext;
    }
     
    public ICollection<Book> GetAllBooks()
    {
      return _applicationDBContext.Books.ToList();
    }

    public Book GetBook(int bookId)
    {
      return _applicationDBContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
    }

    public bool BookExists(int bookId)
    {
      return _applicationDBContext.Books.Any(b => b.Id == bookId);
    }

    public bool BookIsbnExists(string bookIsbn)
    {
      return _applicationDBContext.Books.Any(b => b.Isbn == bookIsbn);
    }

    public decimal GetBookRating(int bookId)
    {
      var review = _applicationDBContext.Reviews.Where(r => r.Books.Id == bookId);

      if (review.Count() <= 0) return 0;

      return (decimal)review.Sum(r => r.Rating) / review.Count();
    }

    public bool isDuplicateISBN(string bookIsbn, int bookId)
    {
      var checkBookIsbn = _applicationDBContext.Books.Where(b => b.Isbn.Trim().ToLower() == bookIsbn.Trim().ToLower() 
          && b.Id != bookId).FirstOrDefault();

      return checkBookIsbn == null ? false : true;
    }

    public Book GetBookByIsbn(string bookIsbn)
    {
      return _applicationDBContext.Books.Where(b => b.Isbn == bookIsbn).FirstOrDefault();
    }

    public bool CreateBook(Book book, List<int> authorsId, List<int> categoriesId)
    {
      var authors = _applicationDBContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
      var categories = _applicationDBContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

      _applicationDBContext.Add(book);
      var saveBookFirst = SaveBook();

      if (saveBookFirst)
      {
        foreach (var author in authors)
        {
          var bookAuthor = new BookAuthor(){
            AuthorId = author.Id,
            BookId = book.Id
          };
          _applicationDBContext.Add(bookAuthor);
        }

        foreach (var category in categories)
        {
          var bookCategory = new BookCategory(){
            BookId = book.Id,
            CategoryId = category.Id
          };
          _applicationDBContext.Add(bookCategory);
        }

        return SaveBook(); 
      } else {
        return false;
      }
    }

    public bool UpdateBook(BookDTO bookDTO, List<int> authorsId, List<int> CategoriesId)
    {
      if (authorsId.Any())
      {
        var authorsToDelete = _applicationDBContext.BookAuthors.Where(ba => ba.BookId == bookDTO.Id).ToList();
        _applicationDBContext.RemoveRange(authorsToDelete);

        var authors = _applicationDBContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();

        foreach (var author in authors)
        {
          var bookAuthor = new BookAuthor(){
            BookId = bookDTO.Id,
            AuthorId = author.Id
          };
          _applicationDBContext.Add(bookAuthor);
        }
      }

      if (CategoriesId.Any())
      {
        var categoriesToDelete = _applicationDBContext.BookCategories.Where(c => c.BookId == bookDTO.Id).ToList();
        _applicationDBContext.RemoveRange(categoriesToDelete);

        var categories = _applicationDBContext.Categories.Where(c => CategoriesId.Contains(c.Id)).ToList();
        
        foreach (var category in categories)
        {
          var bookCategory = new BookCategory(){
            BookId = bookDTO.Id,
            CategoryId = category.Id
          };
          _applicationDBContext.Add(bookCategory);
        }
      }

      var getBook = _applicationDBContext.Books.Where(b => b.Id == bookDTO.Id).First();

      getBook.Isbn = bookDTO.Isbn;
      getBook.DatePublished = bookDTO.DatePublished;
      getBook.Title = bookDTO.Title;
      
      _applicationDBContext.Update(getBook);

      return SaveBook();
    }

    public bool DeleteBook(Book book)
    {
      var reviews = _applicationDBContext.Reviews.Where(r => r.BookId == book.Id).ToList();

      foreach (var review in reviews)
      {
          _applicationDBContext.Remove(review);
      }

      var bookAuthors = _applicationDBContext.BookAuthors.Where(ba => ba.BookId == book.Id).ToList();
      foreach (var bookAuthor in bookAuthors)
      {
          _applicationDBContext.Remove(bookAuthor);
      }

      var bookCategories = _applicationDBContext.BookCategories.Where(bc => bc.BookId == book.Id).ToList();
      foreach (var bookCategory in bookCategories)
      {
        _applicationDBContext.Remove(bookCategory);
      }

      _applicationDBContext.Remove(book);

      return SaveBook();
    }


    public bool SaveBook()
    {
      var book = _applicationDBContext.SaveChanges();
      return book >= 0 ? true : false;
    }
  }
}