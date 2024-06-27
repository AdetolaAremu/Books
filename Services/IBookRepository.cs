using bookreview.Models;

namespace bookreview.Services
{
  public interface IBookRepository
  {
    ICollection<Book> GetAllBooks();
    Book GetBook(int bookId);
    Book GetBookByIsbn(string bookIsbn);
    bool isDuplicateISBN(string bookIsbn, int bookId);
    decimal GetBookRating(int bookId);
    bool BookExists(int bookId);
    bool BookIsbnExists(string bookIsbn);
  }
}