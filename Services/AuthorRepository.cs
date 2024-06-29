using bookreview.DataStore;
using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public class AuthorRepository : IAuthorRepository
  {
    private ApplicationDbContext _authorDBContext;

    public AuthorRepository(ApplicationDbContext applicationDbContext)
    {
      _authorDBContext = applicationDbContext;
    }

    public ICollection<Author> GetAllAuthors()
    {
      return _authorDBContext.Authors.ToList();
    }

    public Author GetAuthor(int authorId)
    {
      return _authorDBContext.Authors.Where(a => a.Id == authorId).FirstOrDefault();
    }

    public ICollection<Author> GetAuthorsOfABook(int bookId)
    {
      return _authorDBContext.BookAuthors.Where(ba => ba.BookId == bookId).Select(a => a.Author).ToList();
    }

    public ICollection<Book> GetBookByAuthor(int authorId)
    {
      return _authorDBContext.BookAuthors.Where(a => a.AuthorId == authorId).Select(b => b.Book).ToList();
    }

    public bool AuthorExists(int authorId)
    {
      return _authorDBContext.Authors.Any(a => a.Id == authorId);
    }

    public bool CreateAuthor(Author author)
    {
      _authorDBContext.Authors.Add(author);

      return SaveAuthor();
    }

    public bool UpdateAuthor(AuthorDTO authorDTO)
    {
      var author = _authorDBContext.Authors.Where(a => a.Id == authorDTO.Id).First();

      author.FirstName = authorDTO.FirstName;
      author.LastName = authorDTO.LastName;
      author.CountryId = authorDTO.CountryId;

      return SaveAuthor();
    }

    public bool DeleteAuthor(Author author)
    {
      _authorDBContext.Remove(author);
      return SaveAuthor();
    }

    public bool SaveAuthor()
    {
      var authorSave = _authorDBContext.SaveChanges();
      return authorSave >= 0 ? true : false;
    }
  }
}