using FlagExplorerBackend.DTOs;
using FlagExplorerBackend.Models;

namespace FlagExplorerBackend.Services;
public interface ICountryService
{
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<CountryDetailsDto> GetCountryDetailsAsync(string name);
}
