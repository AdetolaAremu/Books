using bookreview.DataStore;
using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public class CategoryRepository: ICategoryRepository
  {
    ApplicationDbContext _applicationDBContext;

    public CategoryRepository(ApplicationDbContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public ICollection<Category> GetCategories()
    {
      return _applicationDBContext.Categories.OrderBy(c => c.Name).ToList();
    }

    public Category GetCategory(int categoryId)
    {
      return _applicationDBContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
    }

    public ICollection<Category> GetAllCategoriesOfABook(int bookId)
    {
      return _applicationDBContext.BookCategories.Where(b => b.BookId == bookId).Select(c => c.Category).ToList();
    }

    public ICollection<Book> GetAllBooksPerCategory(int categoryId)
    {
      return _applicationDBContext.BookCategories.Where(bc => bc.CategoryId == categoryId).Select(b => b.Book).ToList();
    }

    public bool CategoryExists(int categoryId)
    {
      return _applicationDBContext.Categories.Any(c => c.Id == categoryId);
    }

    public bool CategoryDuplicate(string name, int categoryId)
    {
      var category = _applicationDBContext.Categories.Where(c => c.Name.Trim().ToLower() == name.Trim().ToLower() 
        && c.Id != categoryId).FirstOrDefault();

      return category == null ? false : true;
    }

    public bool CategoryNameExists(string name)
    {
      var category = _applicationDBContext.Categories.Where(c => c.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();
      return category == null ? false : true;
    }

    public bool CreateCategory(Category category)
    {
      _applicationDBContext.Add(category);
      return SaveCategory();
    }

    public bool UpdateCategory(CategoryDTO categoryDTO)
    {
      var category = _applicationDBContext.Categories.Where(c => c.Id == categoryDTO.Id).FirstOrDefault();

      category.Name = categoryDTO.Name;
      
      return SaveCategory();
    }

    public bool DeleteCategory(Category category)
    {
      _applicationDBContext.Remove(category);
      return SaveCategory();
    }

    public bool SaveCategory()
    {
      var category = _applicationDBContext.SaveChanges();
      return category >= 0 ? true : false;
    }

    public int GetCountOfCategoriesPassed(List<int> categoriesId)
    {
      return _applicationDBContext.Categories.Where(c => categoriesId.Contains(c.Id)).Count();
    }
  }
}