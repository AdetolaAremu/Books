using bookreview.Models;

namespace bookreview.Services
{
  public interface IAuthorRepository
  {
    ICollection<Author> GetAllAuthors();
    Author GetAuthor(int authorId);
    ICollection<Author> GetAuthorsOfABook(int bookId);
    ICollection<Book> GetBookByAuthor(int authorId);
    bool AuthorExists(int authorId);
  }
}