using FlagExplorerBackend.Controllers;
using FlagExplorerBackend.DTOs;
using FlagExplorerBackend.Models;
using FlagExplorerBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FlagExplorerBackend.Tests.Controllers;
public class CountriesControllerTests
{
    private readonly Mock<ICountryService> _mockService;
    private readonly Mock<ILogger<CountriesController>> _mockLogger;
    private readonly CountriesController _controller;

    public CountriesControllerTests()
    {
        _mockService = new Mock<ICountryService>();
        _mockLogger = new Mock<ILogger<CountriesController>>();
        _controller = new CountriesController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllCountries_ReturnsOkResult_WithListOfCountries()
    {
        // Arrange
        var countries = new List<Country>
            {
                new Country { Name = "South Africa" },
                new Country { Name = "Nigeria" }
            };
        _mockService.Setup(s => s.GetAllCountriesAsync())
                    .ReturnsAsync(countries);

        // Act
        var result = await _controller.GetAllCountries();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Country>>(okResult.Value);
        Assert.Equal(2, ((List<Country>)returnValue).Count);
    }

    [Fact]
    public async Task GetCountryDetails_ExistingCountry_ReturnsOkResult()
    {
        // Arrange
        var countryDetails = new CountryDetailsDto
        {
            Name = "South Africa",
            Capital = "Pretoria",
            Population = 60000000,
            Flag = "https://example.com/southafrica.png"
        };
        _mockService.Setup(s => s.GetCountryDetailsAsync("South Africa"))
                    .ReturnsAsync(countryDetails);

        // Act
        var result = await _controller.GetCountryDetails("South Africa");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<CountryDetailsDto>(okResult.Value);
        Assert.Equal("South Africa", returnValue.Name);
    }

    [Fact]
    public async Task GetCountryDetails_NonExistingCountry_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetCountryDetailsAsync("UnknownCountry"))
                    .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.GetCountryDetails("UnknownCountry");

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(404, statusCodeResult.StatusCode);
        Assert.Equal("Country Not Found", statusCodeResult.Value);
    }
}
