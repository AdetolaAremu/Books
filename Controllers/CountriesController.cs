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
    public IActionResult GetAllCountries()
    {
      var countries = _countryRepository.GetCountries().ToList();

      return Ok(countries);
    }
  }
}