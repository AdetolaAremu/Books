using bookreview.DataStore;
using bookreview.DTO;
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

    public bool CategoryNameExists(string name)
    {
      var category = _categoryContext.Categories.Where(c => c.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();
      return category == null ? false : true;
    }

    public bool CreateCategory(Category category)
    {
      _categoryContext.Add(category);
      return SaveCategory();
    }

    public bool UpdateCategory(CategoryDTO categoryDTO)
    {
      var category = _categoryContext.Categories.Where(c => c.Id == categoryDTO.Id).FirstOrDefault();

      category.Name = categoryDTO.Name;
      
      return SaveCategory();
    }

    public bool DeleteCategory(Category category)
    {
      _categoryContext.Remove(category);
      return SaveCategory();
    }

    public bool SaveCategory()
    {
      var category = _categoryContext.SaveChanges();
      return category >= 0 ? true : false;
    }
  }
}