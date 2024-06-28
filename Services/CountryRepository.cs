using bookreview.DataStore;
using bookreview.DTO;
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

    public bool DuplicateCountry(string name, int countryId)
    {
      var country = _countryContext.Countries.Where(c => c.Name.Trim().ToLower() == name.Trim().ToLower() 
        && c.Id != countryId).FirstOrDefault();

      return country == null ? false : true;
    }

    public bool CreateCountry(Country country)
    {
      _countryContext.Add(country);
      return Save();
    }

    public bool UpdateCountry(CountryDTO country)
    {
      var getCountry = _countryContext.Countries.Where(c => c.Id == country.Id).FirstOrDefault();

      getCountry.Name = country.Name;

      return Save();
    }

    public bool DeleteCountry(Country country)
    {
      // var country = _countryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
      _countryContext.Remove(country);
      return Save();
    }

    public bool CheckIfCountryNameExists(string name)
    {
      var country = _countryContext.Countries.Where(c => c.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();

      return country == null ? false : true;
    }

    public bool Save()
    {
      var checkSaved = _countryContext.SaveChanges();
      return checkSaved >= 0 ? true : false;
    }
  }
}