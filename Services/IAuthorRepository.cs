using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public interface IAuthorRepository
  {
    ICollection<Author> GetAllAuthors();
    Author GetAuthor(int authorId);
    ICollection<Author> GetAuthorsOfABook(int bookId);
    ICollection<Book> GetBookByAuthor(int authorId);
    int GetCountOfAuthorsPassed(List<int> authorsId);
    bool AuthorExists(int authorId);
    bool CreateAuthor(Author reviewer);
    bool UpdateAuthor(AuthorDTO authorDTO);
    bool DeleteAuthor(Author reviewer);
    bool SaveAuthor();
  }
}