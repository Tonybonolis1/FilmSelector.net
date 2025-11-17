using MarineTraffic.Application.Interfaces;
using MarineTraffic.Application.Services;
using MarineTraffic.Domain.Common;
using MarineTraffic.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarineTraffic.Tests.Services;

/// <summary>
/// Tests para VesselService
/// Demuestra el uso de mocking para aislar la lógica de negocio
/// </summary>
public class VesselServiceTests
{
    private readonly Mock<IMarineTrafficClient> _mockClient;
    private readonly Mock<ILogger<VesselService>> _mockLogger;
    private readonly VesselService _service;

    public VesselServiceTests()
    {
        _mockClient = new Mock<IMarineTrafficClient>();
        _mockLogger = new Mock<ILogger<VesselService>>();
        _service = new VesselService(_mockClient.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task SearchVesselsAsync_WithValidQuery_ReturnsVessels()
    {
        // Arrange
        var query = "MAERSK";
        var expectedVessels = new List<Vessel>
        {
            new Vessel
            {
                Id = "1",
                Name = "MAERSK TEST",
                Mmsi = "123456789",
                Imo = "1234567",
                ShipType = "Cargo",
                Flag = "DK"
            }
        };

        _mockClient
            .Setup(c => c.SearchVesselsAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<Vessel>>.Success(expectedVessels));

        // Act
        var (success, data, error) = await _service.SearchVesselsAsync(query);

        // Assert
        Assert.True(success);
        Assert.NotNull(data);
        Assert.Single(data);
        Assert.Equal("MAERSK TEST", data[0].Name);
        Assert.Null(error);

        // Verificar que se llamó al cliente
        _mockClient.Verify(
            c => c.SearchVesselsAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task SearchVesselsAsync_WithEmptyQuery_ReturnsError()
    {
        // Arrange
        var query = "";

        // Act
        var (success, data, error) = await _service.SearchVesselsAsync(query);

        // Assert
        Assert.False(success);
        Assert.Null(data);
        Assert.NotNull(error);
        Assert.Contains("vacío", error);

        // Verificar que NO se llamó al cliente
        _mockClient.Verify(
            c => c.SearchVesselsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task SearchVesselsAsync_WhenClientFails_ReturnsError()
    {
        // Arrange
        var query = "TEST";
        var errorMessage = "API Error";

        _mockClient
            .Setup(c => c.SearchVesselsAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<Vessel>>.Failure(errorMessage));

        // Act
        var (success, data, error) = await _service.SearchVesselsAsync(query);

        // Assert
        Assert.False(success);
        Assert.Null(data);
        Assert.Equal(errorMessage, error);
    }

    [Fact]
    public async Task SearchVesselsAsync_WhenNoVesselsFound_ReturnsEmptyList()
    {
        // Arrange
        var query = "NONEXISTENT";
        var emptyList = new List<Vessel>();

        _mockClient
            .Setup(c => c.SearchVesselsAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<Vessel>>.Success(emptyList));

        // Act
        var (success, data, error) = await _service.SearchVesselsAsync(query);

        // Assert
        Assert.True(success);
        Assert.NotNull(data);
        Assert.Empty(data);
        Assert.Null(error);
    }

    [Fact]
    public async Task GetVoyageInfoAsync_WithValidVesselId_ReturnsVoyageInfo()
    {
        // Arrange
        var vesselId = "123";
        var expectedVoyage = new VoyageInfo
        {
            VesselId = vesselId,
            DestinationPort = "SANTA MARTA",
            DestinationPortUnlocode = "COSMR",
            DestinationCountry = "Colombia",
            EstimatedTimeOfArrival = DateTime.UtcNow.AddDays(2),
            CurrentLatitude = 11.2472,
            CurrentLongitude = -74.2017,
            CurrentSpeed = 15.5
        };

        _mockClient
            .Setup(c => c.GetVoyageInfoAsync(vesselId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<VoyageInfo>.Success(expectedVoyage));

        // Act
        var (success, data, error) = await _service.GetVoyageInfoAsync(vesselId);

        // Assert
        Assert.True(success);
        Assert.NotNull(data);
        Assert.Equal("SANTA MARTA", data.DestinationPort);
        Assert.True(data.IsDestinationSantaMarta);
        Assert.Null(error);
    }

    [Fact]
    public async Task GetVoyageInfoAsync_WithEmptyVesselId_ReturnsError()
    {
        // Arrange
        var vesselId = "";

        // Act
        var (success, data, error) = await _service.GetVoyageInfoAsync(vesselId);

        // Assert
        Assert.False(success);
        Assert.Null(data);
        Assert.NotNull(error);
        Assert.Contains("vacío", error);
    }

    [Fact]
    public async Task GetVoyageInfoAsync_WhenVoyageNotFound_ReturnsError()
    {
        // Arrange
        var vesselId = "999";

        _mockClient
            .Setup(c => c.GetVoyageInfoAsync(vesselId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<VoyageInfo>.Failure("No encontrado", "NOT_FOUND"));

        // Act
        var (success, data, error) = await _service.GetVoyageInfoAsync(vesselId);

        // Assert
        Assert.False(success);
        Assert.Null(data);
        Assert.NotNull(error);
    }
}
