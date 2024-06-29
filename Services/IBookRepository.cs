using bookreview.DTO;
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

    bool CreateBook(Book book, List<int> authorId, List<int> CategoryId);
    bool UpdateBook(BookDTO bookDTO, List<int> authorId, List<int> CategoryId);
    bool DeleteBook(Book book);
    bool SaveBook();
  }
}