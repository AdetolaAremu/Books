using bookreview.DataStore;
using bookreview.Models;

namespace bookreview.Services
{
  public class CategoryRepository: ICategoryRepository
  {
    ApplicationDbContext _categoryContext;

    public CategoryRepository(ApplicationDbContext categoryContext)
    {
      _categoryContext = categoryContext;
    }

    public ICollection<Category> GetCategories()
    {
      return _categoryContext.Categories.OrderBy(c => c.Name).ToList();
    }

    public Category GetCategory(int categoryId)
    {
      return _categoryContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public ICollection<Category> GetAllCategoriesOfABook(int bookId)
    {
      return _categoryContext.BookCategories.Where(b => b.BookId == bookId).Select(c => c.Category).ToList();
    }

    public ICollection<Book> GetAllBooksPerCategory(int categoryId)
    {
      return _categoryContext.BookCategories.Where(bc => bc.CategoryId == categoryId).Select(b => b.Book).ToList();
    }

    public bool CategoryExists(int categoryId)
    {
      return _categoryContext.Categories.Any(c => c.Id == categoryId);
    }

    public bool CategoryDuplicate(string name, int categoryId)
    {
      var category = _categoryContext.Categories.Where(c => c.Name.Trim().ToLower() == name.Trim().ToLower() 
        && c.Id != categoryId).FirstOrDefault();

      return category == null ? false : true;
    }
  }
}