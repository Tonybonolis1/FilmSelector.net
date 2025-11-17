using MarineTraffic.Application.Interfaces;
using MarineTraffic.Application.Services;
using MarineTraffic.Domain.Common;
using MarineTraffic.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarineTraffic.Tests.Services;

/// <summary>
/// Tests para PortService
/// </summary>
public class PortServiceTests
{
    private readonly Mock<IMarineTrafficClient> _mockClient;
    private readonly Mock<ILogger<PortService>> _mockLogger;
    private readonly PortService _service;

    public PortServiceTests()
    {
        _mockClient = new Mock<IMarineTrafficClient>();
        _mockLogger = new Mock<ILogger<PortService>>();
        _service = new PortService(_mockClient.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetSantaMartaArrivalsAsync_ReturnsArrivals()
    {
        // Arrange
        var expectedArrivals = new List<PortArrival>
        {
            new PortArrival
            {
                VesselId = "1",
                VesselName = "TEST VESSEL 1",
                Mmsi = "111111111",
                EstimatedTimeOfArrival = DateTime.UtcNow.AddDays(1),
                DestinationPort = "Santa Marta"
            },
            new PortArrival
            {
                VesselId = "2",
                VesselName = "TEST VESSEL 2",
                Mmsi = "222222222",
                EstimatedTimeOfArrival = DateTime.UtcNow.AddDays(2),
                DestinationPort = "Santa Marta"
            }
        };

        _mockClient
            .Setup(c => c.GetPortArrivalsAsync("COSMR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<PortArrival>>.Success(expectedArrivals));

        // Act
        var (success, data, error) = await _service.GetSantaMartaArrivalsAsync();

        // Assert
        Assert.True(success);
        Assert.NotNull(data);
        Assert.Equal(2, data.Count);
        Assert.Null(error);
    }

    [Fact]
    public async Task GetSantaMartaArrivalsAsync_WhenNoArrivals_ReturnsEmptyList()
    {
        // Arrange
        var emptyList = new List<PortArrival>();

        _mockClient
            .Setup(c => c.GetPortArrivalsAsync("COSMR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<PortArrival>>.Success(emptyList));

        // Act
        var (success, data, error) = await _service.GetSantaMartaArrivalsAsync();

        // Assert
        Assert.True(success);
        Assert.NotNull(data);
        Assert.Empty(data);
        Assert.Null(error);
    }

    [Fact]
    public async Task GetSantaMartaArrivalsAsync_WhenClientFails_ReturnsError()
    {
        // Arrange
        var errorMessage = "Connection failed";

        _mockClient
            .Setup(c => c.GetPortArrivalsAsync("COSMR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<List<PortArrival>>.Failure(errorMessage));

        // Act
        var (success, data, error) = await _service.GetSantaMartaArrivalsAsync();

        // Assert
        Assert.False(success);
        Assert.Null(data);
        Assert.Equal(errorMessage, error);
    }
}
