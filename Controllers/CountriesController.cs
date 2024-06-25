using bookreview.DTO;
using bookreview.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookreview.Controllers
{
  [Route("api/countries")]
  [ApiController]
  public class CountryController : ControllerBase
  {
    private ICountryRepository _countryRepository;

    public CountryController(ICountryRepository country)
    {
      _countryRepository = country;
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

      return Ok(countryDTO);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetOneCountry(int countryId)
    {
      if (!_countryRepository.CountryExists(countryId)) return NotFound();

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
      var authorCountry = _countryRepository.GetCountryOfAnAuthor(authorId);

      if (authorCountry == null) return NotFound();

      if(!ModelState.IsValid) return BadRequest();

      var countryDTO = new CountryDTO(){
        Id = authorCountry.Id,
        Name = authorCountry.Name
      };

      return Ok(countryDTO);
    }

    [HttpGet("{countryId}/author")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAuthorsFromACountry(int countryId)
    { 
      if (!_countryRepository.CountryExists(countryId)) return NotFound();

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

      return Ok(authorDTO);
    }
  }
}