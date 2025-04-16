using FlagExplorerBackend.Converters;
using FlagExplorerBackend.DTOs;
using FlagExplorerBackend.Models;
using System.Text.Json;

namespace FlagExplorerBackend.Services;
public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new CountryJsonConverter(), new CountryDetailsJsonConverter() }
    };

    public CountryService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Country>>("all", _options);
        return response ?? new List<Country>();
    }

    public async Task<CountryDetailsDto> GetCountryDetailsAsync(string name)
    {
        var response = await _httpClient.GetFromJsonAsync<List<CountryDetailsDto>>($"name/{name}", _options);
        return response?.FirstOrDefault(c => c.Name == name) ?? new CountryDetailsDto();
    }
}