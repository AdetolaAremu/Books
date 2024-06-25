using bookreview.DTO;
using bookreview.Models;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [Route("/api/catgories")]
  [ApiController]
  public class CategoryController : ControllerBase
  {
    private ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
      _categoryRepository = categoryRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllCategories()
    {
      var categories = _categoryRepository.GetCategories().ToList();

      if (!ModelState.IsValid) return BadRequest();

      var categoryDTO = new List<CategoryDTO>();

      foreach (var category in categories)
      {
        categoryDTO.Add(new CategoryDTO{
          Id = category.Id,
          Name = category.Name,
          CreatedAt = category.CreatedAt,
          UpdatedAt = category.UpdatedAt
        });
      }

      return Ok(categoryDTO);
    }
    
    [HttpGet("{bookId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetACategory(int bookId)
    {
      var checkcategory = _categoryRepository.CategoryExists(bookId);

      if (!checkcategory) return NotFound();

      var category = _categoryRepository.GetCategory(bookId);

      if (!ModelState.IsValid) return BadRequest();

      var categoryDTO = new CategoryDTO(){
        Id = category.Id,
        Name = category.Name,
        CreatedAt = category.CreatedAt,
        UpdatedAt = category.UpdatedAt
      };

      return Ok(categoryDTO);
    }

    [HttpGet("book-categories/{bookId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCategoriesOfABook(int bookId)
    {
      var checkcategory = _categoryRepository.CategoryExists(bookId);

      if (!checkcategory) return NotFound();

      var categories = _categoryRepository.GetAllCategoriesOfABook(bookId);

      var categoryDTO = new List<CategoryDTO>();

      foreach (var category in categories)
      {
        categoryDTO.Add(new CategoryDTO{
           Id = category.Id,
          Name = category.Name,
          CreatedAt = category.CreatedAt,
          UpdatedAt = category.UpdatedAt
        });
      }

      return Ok(categories);
    }
  }
}