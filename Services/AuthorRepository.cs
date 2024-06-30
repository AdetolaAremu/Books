using bookreview.DataStore;
using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public class AuthorRepository : IAuthorRepository
  {
    private ApplicationDbContext __applicationDBContext;

    public AuthorRepository(ApplicationDbContext applicationDbContext)
    {
      __applicationDBContext = applicationDbContext;
    }

    public ICollection<Author> GetAllAuthors()
    {
      return __applicationDBContext.Authors.ToList();
    }

    public Author GetAuthor(int authorId)
    {
      return __applicationDBContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
    }

    public ICollection<Author> GetAuthorsOfABook(int bookId)
    {
      return __applicationDBContext.BookAuthors.Where(ba => ba.BookId == bookId).Select(a => a.Author).ToList();
    }

    public ICollection<Book> GetBookByAuthor(int authorId)
    {
      return __applicationDBContext.BookAuthors.Where(a => a.AuthorId == authorId).Select(b => b.Book).ToList();
    }

    public bool AuthorExists(int authorId)
    {
      return __applicationDBContext.Authors.Any(a => a.Id == authorId);
    }

    public bool CreateAuthor(Author author)
    {
      __applicationDBContext.Authors.Add(author);

      return SaveAuthor();
    }

    public bool UpdateAuthor(AuthorDTO authorDTO)
    {
      var author = __applicationDBContext.Authors.Where(a => a.Id == authorDTO.Id).First();

      author.FirstName = authorDTO.FirstName;
      author.LastName = authorDTO.LastName;
      author.CountryId = authorDTO.CountryId;

      return SaveAuthor();
    }

    public bool DeleteAuthor(Author author)
    {
      __applicationDBContext.Remove(author);
      return SaveAuthor();
    }

    public bool SaveAuthor()
    {
      var authorSave = __applicationDBContext.SaveChanges();
      return authorSave >= 0 ? true : false;
    }

    public int GetCountOfAuthorsPassed(List<int> authorsId)
    {
      return __applicationDBContext.Authors.Where(a => authorsId.Contains(a.Id)).Count();
    }
  }
}