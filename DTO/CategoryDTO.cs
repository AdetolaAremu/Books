using bookreview.Models;

namespace bookreview.DTO
{
  public class CategoryDTO
  {
    public int Id { get; set; }
    public string Name { get; set; }
    // public virtual ICollection<BookCategory> BookCategories { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}