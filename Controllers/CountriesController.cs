using bookreview.DTO;
using bookreview.Helpers;
using bookreview.Models;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [Route("api/countries")]
  [ApiController]
  public class CountryController : ControllerBase
  {
    private ICountryRepository _countryRepository;
    private IAuthorRepository _authorRepository;

    public CountryController(ICountryRepository country, IAuthorRepository authorRepository)
    {
      _countryRepository = country;
      _authorRepository = authorRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllCountries()
    {
      var countries = _countryRepository.GetCountries().ToList();

      if (!ModelState.IsValid) return BadRequest();

      var countryDTO = new List<CountryDTO>();
      foreach(var country in countries)
      {
        countryDTO.Add(new CountryDTO{
          Id = country.Id,
          Name = country.Name
        });
      }

      return ResponseHelper.SuccessResponseHelper("Countries retrieved successfuly", countryDTO, 200);
      // return Ok(countryDTO);
    }

    [HttpGet("{countryId}", Name = "GetOneCountry")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetOneCountry(int countryId)
    {
      if (!_countryRepository.CountryExists(countryId)) return ResponseHelper.ErrorResponseHelper($"Country with id: {countryId} does not exist", null, 404);

      var country = _countryRepository.GetCountry(countryId);

      if (!ModelState.IsValid) return BadRequest();

      var countryDTO = new CountryDTO(){
        Id = country.Id,
        Name = country.Name
      };

      return Ok(countryDTO);
    }

    [HttpGet("author/{authorId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCountryOfAnAuthor(int authorId)
    {
      if (!_authorRepository.AuthorExists(authorId)) return ResponseHelper.ErrorResponseHelper("Author does not exist", null, 404);

      var authorCountry = _countryRepository.GetCountryOfAnAuthor(authorId);

      if(!ModelState.IsValid) return BadRequest();

      var countryDTO = new CountryDTO(){
        Id = authorCountry.Id,
        Name = authorCountry.Name
      };

      return ResponseHelper.SuccessResponseHelper("Country of author retrieved successfully", countryDTO, 200);
      // return Ok(countryDTO);
    }

    [HttpGet("{countryId}/author")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAuthorsFromACountry(int countryId)
    { 
      if (!_countryRepository.CountryExists(countryId))
          return ResponseHelper.ErrorResponseHelper("Country does not exist", "", 404);

      var authors = _countryRepository.GetAuthorsFromACountry(countryId);

      if(!ModelState.IsValid) return BadRequest();

      var authorDTO = new List<AuthorDTO>();

      foreach(var author in authors)
      {
        authorDTO.Add(new AuthorDTO{
          Id = author.Id,
          FirstName = author.FirstName,
          LastName = author.LastName,
        });
      }

      return ResponseHelper.SuccessResponseHelper("Countries of authors retrieved successfully", authorDTO, 200);
      // return Ok(authorDTO);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CountryDTO))]
    public IActionResult CreateCountry([FromBody] CountryDTO countryRequest)
    {
      if (countryRequest == null) 
      return ResponseHelper.ErrorResponseHelper("Request cannot be processed", new { ModelState }, 422); //UnprocessableEntity(ModelState);

      var checkIfCountryNameExists = _countryRepository.CheckIfCountryNameExists(countryRequest.Name);

      if (checkIfCountryNameExists == true) {
        ResponseHelper.ErrorResponseHelper($"Country name: {countryRequest.Name} already exists", ModelState);
        // ModelState.AddModelError("", $"Country name: {countryRequest.Name} already exists");
        // return BadRequest(ModelState);
      }

      if (!ModelState.IsValid) return BadRequest(ModelState);

      var createCountryRequest = _countryRepository.CreateCountry(new Country{
        Name = countryRequest.Name
      });

      if (!createCountryRequest)
      {
        ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record {countryRequest.Name}", new {ModelState});
        // ModelState.AddModelError("", $"Something went wrong while saving {countryRequest.Name}");
        // return BadRequest(ModelState);
      }

      return ResponseHelper.SuccessResponseHelper("Country created successfully", countryRequest, 201);
      // return CreatedAtRoute("GetOneCountry", new{ countryId = countryRequest.Id }, countryRequest);
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountryDTO))]
    public IActionResult UpdateCountry(int countryId, [FromBody] CountryDTO countryDTO)
    {
      if (countryDTO == null) return ResponseHelper.ErrorResponseHelper("Request body cannot be empty", null, 400);

      if (countryId != countryDTO.Id) return ResponseHelper.ErrorResponseHelper("Invalid request", null, 400);

      if (!_countryRepository.CountryExists(countryId)) return ResponseHelper.ErrorResponseHelper("Country does not exists", null, 404);

      if (_countryRepository.DuplicateCountry(countryDTO.Name, countryId)) return 
          ResponseHelper.ErrorResponseHelper($"Duplicate record for country name: {countryDTO.Name}");

      if (!ModelState.IsValid) BadRequest(ModelState);

      var updateCountry = _countryRepository.UpdateCountry(countryDTO);

      if (!updateCountry) ResponseHelper.ErrorResponseHelper($"Something went wrong while saving your record, please retry", new {ModelState});

      return ResponseHelper.SuccessResponseHelper("Country updated successfully", countryDTO, 200);
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteCountry(int countryId)
    {
      if (!_countryRepository.CountryExists(countryId)) 
          return ResponseHelper.ErrorResponseHelper("Country does not exist", "", 404);

      if (_countryRepository.GetAuthorsFromACountry(countryId).Count() > 0)
        return ResponseHelper.ErrorResponseHelper("There are authors belonging to this country", null, 400);

      var country = _countryRepository.GetCountry(countryId);

      if (!ModelState.IsValid) return BadRequest();

      var deleteCountry = _countryRepository.DeleteCountry(country);

      if (!deleteCountry) ResponseHelper.ErrorResponseHelper($"Something went wrong while deleting your record, please retry", new {ModelState});

      return ResponseHelper.SuccessResponseHelper<string>("Country deleted successfully", null, 200);      
    }
  }
}