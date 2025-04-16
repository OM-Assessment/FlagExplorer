using FlagExplorerBackend.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace FlagExplorerBackend.Tests.Integration;
public class CountryServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CountryServiceIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllCountries_ReturnsSuccessAndData()
    {
        var response = await _client.GetAsync("/api/countries");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var countries = await response.Content.ReadFromJsonAsync<IEnumerable<Country>>();
        Assert.NotNull(countries);
        Assert.NotEmpty(countries);
    }

    [Fact]
    public async Task GetCountryDetails_ReturnsSpecificCountry()
    {
        var response = await _client.GetAsync("/api/countries/South Africa");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var country = await response.Content.ReadFromJsonAsync<Country>();
        Assert.NotNull(country);
        Assert.Equal("South Africa", country.Name);
    }

    [Fact]
    public async Task GetCountryDetails_ReturnsNotFound_ForInvalidCountry()
    {
        var response = await _client.GetAsync("/api/countries/No Country");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }
}