using bookreview.Models;

namespace bookreview.Services
{
  public interface ICountryRepository
  {
    ICollection<Country> GetCountries();
    Country GetCountry(int countryId);
    Country GetCountryOfAnAuthor(int authorId);
    ICollection<Author> GetAuthorsFromACountry(int countryId);
  }
}