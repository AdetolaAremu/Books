using bookreview.DataStore;
using bookreview.Models;

namespace bookreview.Services
{
  public class CountryRepository : ICountryRepository
  {
    private ApplicationDbContext _countryContext;

    public CountryRepository(ApplicationDbContext countryContext)
    {
      _countryContext = countryContext;
    }

    public ICollection<Author> GetAuthorsFromACountry(int countryId)
    {
      return _countryContext.Authors.Where(a => a.Id == countryId).ToList();
    }

    public bool CountryExists(int countryId)
    {
      return _countryContext.Countries.Any(c => c.Id == countryId);
    }

    public ICollection<Country> GetCountries()
    {
      return _countryContext.Countries.OrderBy(c => c.Name).ToList();
    }

    public Country GetCountry(int countryId)
    {
      return _countryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
    }

    public Country GetCountryOfAnAuthor(int authorId)
    {
      return _countryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
    }
  }
}