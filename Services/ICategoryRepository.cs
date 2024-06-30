using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public interface ICategoryRepository
  {
    ICollection<Category> GetCategories();
    Category GetCategory(int categoryId);
    ICollection<Category> GetAllCategoriesOfABook(int bookId);
    ICollection<Book> GetAllBooksPerCategory(int categoryId);
    int GetCountOfCategoriesPassed(List<int> categoriesId);
    bool CategoryExists(int categoryId);
    bool CategoryDuplicate(string name, int categoryId);
    bool CategoryNameExists(string name);
    bool CreateCategory(Category category);
    bool UpdateCategory(CategoryDTO categoryDTO);
    bool DeleteCategory(Category category);
    bool SaveCategory();
  }
}