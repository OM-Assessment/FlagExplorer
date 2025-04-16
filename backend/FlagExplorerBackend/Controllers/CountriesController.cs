using FlagExplorerBackend.DTOs;
using FlagExplorerBackend.Models;
using FlagExplorerBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlagExplorerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _countryService;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(ICountryService countryService, ILogger<CountriesController> logger)
    {
        _countryService = countryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
    {
        try
        {
            var countries = await _countryService.GetAllCountriesAsync();
            _logger.LogInformation("Countries data successfully fetched");
            return Ok(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all countries");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<CountryDetailsDto>> GetCountryDetails(string name)
    {
        try
        {
            var country = await _countryService.GetCountryDetailsAsync(name);
            _logger.LogInformation($"Successfully loaded details for country: {country.Name}");
            return Ok(country);
        }

        catch (Exception)
        {
            _logger.LogError($"Country not found: {name}");
            return StatusCode(404, "Country Not Found");
        }

    }

}
