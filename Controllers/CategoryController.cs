using bookreview.DTO;
using bookreview.Helpers;
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

    [HttpGet("{categoryId}/books")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBooksFromACategory(int categoryId)
    {
      var checkcategory = _categoryRepository.CategoryExists(categoryId);

      if (!checkcategory) return NotFound();

      var books = _categoryRepository.GetAllBooksPerCategory(categoryId);

      if (!ModelState.IsValid) return BadRequest();

      var bookDto = new List<BookDTO>();

      foreach (var book in books)
      {
        bookDto.Add(new BookDTO{
          Id = book.Id,
          Isbn = book.Isbn,
          Title = book.Title,
          DatePublished = book.DatePublished
        });
      }

      return Ok(bookDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDTO))]
    public IActionResult CreateCategory([FromBody]CategoryDTO categoryDTO)
    {
      if (categoryDTO == null) return ResponseHelper.ErrorResponseHelper("Invalid request body", new { ModelState }, 422);

      if (_categoryRepository.CategoryNameExists(categoryDTO.Name))
        return ResponseHelper.ErrorResponseHelper($"Category name:{categoryDTO.Name} already exists", null, 400);

      if (!ModelState.IsValid) return BadRequest();

      var saveCategory = _categoryRepository.CreateCategory(new Category{
        Name = categoryDTO.Name
      });

      if (!saveCategory) return ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {categoryDTO}", new {ModelState});

      return ResponseHelper.SuccessResponseHelper("Category created successfully", categoryDTO, 201);
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDTO))]
    public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO categoryDTO)
    {
      if (!_categoryRepository.CategoryExists(categoryId)) return ResponseHelper.ErrorResponseHelper("Category does not exists");

      if (categoryDTO == null) return ResponseHelper.ErrorResponseHelper("Invalid request body", new { ModelState }, 422);

      if (_categoryRepository.CategoryDuplicate(categoryDTO.Name, categoryId)) 
        return ResponseHelper.ErrorResponseHelper($"Duplicate record for category name: {categoryDTO.Name.ToUpper()}");

      if (categoryId != categoryDTO.Id) return ResponseHelper.ErrorResponseHelper("Invalid request");

      if (!ModelState.IsValid) BadRequest(ModelState);

      var category = _categoryRepository.UpdateCategory(categoryDTO);

      if (!category) return ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {categoryDTO}", new {ModelState});

      return ResponseHelper.SuccessResponseHelper("Category updated successfully", categoryDTO);
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteCategory(int categoryId)
    {
      if (!_categoryRepository.CategoryExists(categoryId)) return ResponseHelper.ErrorResponseHelper("Category does not exists");

      if (_categoryRepository.GetAllBooksPerCategory(categoryId).Count() > 0)
        return ResponseHelper.ErrorResponseHelper("There are books belonging to this category", null, 400);

      var category = _categoryRepository.GetCategory(categoryId);

      if (!ModelState.IsValid) BadRequest(ModelState);

      var deleteCategory = _categoryRepository.DeleteCategory(category);

      if (!deleteCategory) ResponseHelper.ErrorResponseHelper($"Something went wrong while deleting your record, please retry", new {ModelState});

      return ResponseHelper.SuccessResponseHelper("Category deleted successfully", null, 200);
    }
  }
}