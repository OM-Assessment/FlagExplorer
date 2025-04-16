using FlagExplorerBackend.Converters;
using FlagExplorerBackend.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace FlagExplorerBackend.Tests.Services;
public class CountryServiceTests
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        Converters = { new CountryJsonConverter(), new CountryDetailsJsonConverter() }
    };

    private HttpClient CreateMockHttpClient(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(response);

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://mock-api/")
        };
    }

    [Fact]
    public async Task GetAllCountriesAsync_ReturnsCountryList()
    {
        // Arrange: simulate expected JSON structure
        var json = """
        [
            {
                "name": { "common": "South Africa" },
                "flags": { "png": "https://flagcdn.com/za.png" }
            },
            {
                "name": { "common": "Nigeria" },
                "flags": { "png": "https://flagcdn.com/ng.png" }
            }
        ]
        """;

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };
        var httpClient = CreateMockHttpClient(response);
        var service = new CountryService(httpClient);

        // Act
        var result = await service.GetAllCountriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.Name == "South Africa");
        Assert.Contains(result, c => c.Name == "Nigeria");
    }

    [Fact]
    public async Task GetCountryDetailsAsync_ReturnsMatchingCountry()
    {
        // Arrange: simulate expected JSON structure
        var json = """
        [
            {
                "name": { "common": "South Africa" },
                "capital": ["Pretoria"],
                "population": 60000000,
                "flags": { "png": "https://example.com/southafrica.png" }
            }
        ]
        """;

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };
        var httpClient = CreateMockHttpClient(response);
        var service = new CountryService(httpClient);

        // Act
        var result = await service.GetCountryDetailsAsync("South Africa");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("South Africa", result.Name);
        Assert.Equal("Pretoria", result.Capital);
        Assert.Equal(60000000, result.Population);
        Assert.Equal("https://example.com/southafrica.png", result.Flag);
    }

    [Fact]
    public async Task GetCountryDetailsAsync_ReturnsEmpty_WhenNoMatch()
    {
        // Arrange: empty list simulates no match
        var json = "[]";

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };
        var httpClient = CreateMockHttpClient(response);
        var service = new CountryService(httpClient);

        // Act
        var result = await service.GetCountryDetailsAsync("Nonexistent");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Name); // Default CountryDetailsDto
    }
}
