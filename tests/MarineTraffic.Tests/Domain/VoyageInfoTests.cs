using MarineTraffic.Domain.Entities;

namespace MarineTraffic.Tests.Domain;

/// <summary>
/// Tests para la lógica de negocio en el dominio
/// </summary>
public class VoyageInfoTests
{
    [Fact]
    public void IsDestinationSantaMarta_WhenDestinationIsSantaMarta_ReturnsTrue()
    {
        // Arrange
        var voyage = new VoyageInfo
        {
            VesselId = "1",
            DestinationPort = "SANTA MARTA"
        };

        // Assert
        Assert.True(voyage.IsDestinationSantaMarta);
    }

    [Fact]
    public void IsDestinationSantaMarta_WhenUnlocodeIsCOSMR_ReturnsTrue()
    {
        // Arrange
        var voyage = new VoyageInfo
        {
            VesselId = "1",
            DestinationPort = "Unknown Port",
            DestinationPortUnlocode = "COSMR"
        };

        // Assert
        Assert.True(voyage.IsDestinationSantaMarta);
    }

    [Fact]
    public void IsDestinationSantaMarta_WhenUnlocodeIsCOSMRWithSpace_ReturnsTrue()
    {
        // Arrange
        var voyage = new VoyageInfo
        {
            VesselId = "1",
            DestinationPortUnlocode = "CO SMR"
        };

        // Assert
        Assert.True(voyage.IsDestinationSantaMarta);
    }

    [Fact]
    public void IsDestinationSantaMarta_WhenDestinationIsOtherPort_ReturnsFalse()
    {
        // Arrange
        var voyage = new VoyageInfo
        {
            VesselId = "1",
            DestinationPort = "CARTAGENA",
            DestinationPortUnlocode = "COCTG"
        };

        // Assert
        Assert.False(voyage.IsDestinationSantaMarta);
    }

    [Fact]
    public void IsDestinationSantaMarta_WhenDestinationIsNull_ReturnsFalse()
    {
        // Arrange
        var voyage = new VoyageInfo
        {
            VesselId = "1",
            DestinationPort = null,
            DestinationPortUnlocode = null
        };

        // Assert
        Assert.False(voyage.IsDestinationSantaMarta);
    }

    [Theory]
    [InlineData("santa marta")]
    [InlineData("SANTA MARTA")]
    [InlineData("Santa Marta, Colombia")]
    [InlineData("Port of Santa Marta")]
    public void IsDestinationSantaMarta_WithDifferentCases_ReturnsTrue(string portName)
    {
        // Arrange
        var voyage = new VoyageInfo
        {
            VesselId = "1",
            DestinationPort = portName
        };

        // Assert
        Assert.True(voyage.IsDestinationSantaMarta);
    }
}
