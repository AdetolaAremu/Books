using bookreview.DataStore;
using bookreview.Models;

namespace bookreview.Services
{
  public class CountryRepository : ICountryRepository
  {
    private ApplicationDbContext _countryContext;

    public ICollection<Author> GetAuthorsFromACountry(int countryId)
    {
      throw new NotImplementedException();
    }

    public ICollection<Country> GetCountries()
    {
      throw new NotImplementedException();
    }

    public Country GetCountry(int countryId)
    {
      throw new NotImplementedException();
    }

    public Country GetCountryOfAnAuthor(int countryId)
    {
      throw new NotImplementedException();
    }
  }
}