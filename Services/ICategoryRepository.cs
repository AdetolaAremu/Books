using bookreview.Models;

namespace bookreview.Services
{
  public interface ICategoryRepository
  {
    ICollection<Category> GetCategories();
    Category GetCategory(int categoryId);
    ICollection<Category> GetAllCategoriesOfABook(int bookId);
    ICollection<Book> GetAllBooksPerCategory(int categoryId);
    bool CategoryExists(int categoryId);
    bool CategoryDuplicate(string name, int categoryId);
  }
}