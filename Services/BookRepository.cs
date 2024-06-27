using bookreview.DataStore;
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
  }
}