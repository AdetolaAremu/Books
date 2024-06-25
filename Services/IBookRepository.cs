using bookreview.Models;

namespace bookreview.Services
{
  public interface IBookRepository
  {
    ICollection<Book> GetAllBooks();
    Book GetBook(int bookId);
    Book GetBookByIsbn(int bookId, string bookIsbn);
    Book isDuplicateISBN(string bookIsbn);
    decimal GetBookRating(int bookId);
    bool BookExists(int bookId);
    bool BookIsbnExists(string bookIsbn);
  }
}