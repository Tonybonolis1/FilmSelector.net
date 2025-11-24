using FilmSelector.Application.Mappings;
using FilmSelector.Domain.Entities;

namespace FilmSelector.Tests.Mappings;

/// <summary>
/// Tests para los mapeos de DTOs
/// Asegura que las transformaciones sean correctas
/// </summary>
public class VesselMappingsTests
{
    [Fact]
    public void ToSearchResponseDto_MapsAllProperties()
    {
        // Arrange
        var vessel = new Vessel
        {
            Id = "123",
            Name = "TEST VESSEL",
            Mmsi = "987654321",
            Imo = "1234567",
            ShipType = "Container Ship",
            Flag = "US"
        };

        // Act
        var dto = vessel.ToSearchResponseDto();

        // Assert
        Assert.Equal(vessel.Id, dto.Id);
        Assert.Equal(vessel.Name, dto.Name);
        Assert.Equal(vessel.Mmsi, dto.Mmsi);
        Assert.Equal(vessel.Imo, dto.Imo);
        Assert.Equal(vessel.ShipType, dto.ShipType);
        Assert.Equal(vessel.Flag, dto.Flag);
    }

    [Fact]
    public void ToSearchResponseDtoList_MapsMultipleVessels()
    {
        // Arrange
        var vessels = new List<Vessel>
        {
            new Vessel { Id = "1", Name = "VESSEL 1" },
            new Vessel { Id = "2", Name = "VESSEL 2" },
            new Vessel { Id = "3", Name = "VESSEL 3" }
        };

        // Act
        var dtos = vessels.ToSearchResponseDtoList();

        // Assert
        Assert.Equal(3, dtos.Count);
        Assert.Equal("VESSEL 1", dtos[0].Name);
        Assert.Equal("VESSEL 2", dtos[1].Name);
        Assert.Equal("VESSEL 3", dtos[2].Name);
    }
}

