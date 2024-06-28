using bookreview.DTO;
using bookreview.Models;

namespace bookreview.Services
{
  public interface ICountryRepository
  {
    ICollection<Country> GetCountries();
    Country GetCountry(int countryId);
    Country GetCountryOfAnAuthor(int authorId);
    ICollection<Author> GetAuthorsFromACountry(int countryId);
    bool CountryExists(int countryId);
    bool DuplicateCountry(string name, int countryId);
    bool CheckIfCountryNameExists(string name);
    bool CreateCountry(Country country);
    bool UpdateCountry(CountryDTO country);
    bool DeleteCountry(Country country);
    bool Save();
  }
}