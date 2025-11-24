using FilmSelector.Application.DTOs.Responses;
using FilmSelector.Domain.Entities;

namespace FilmSelector.Application.Mappings;

/// <summary>
/// Clase estática para mapeo manual de PortArrival a DTOs
/// </summary>
public static class PortArrivalMappings
{
    public static PortArrivalResponseDto ToResponseDto(this PortArrival arrival)
    {
        return new PortArrivalResponseDto
        {
            VesselId = arrival.VesselId,
            VesselName = arrival.VesselName,
            Mmsi = arrival.Mmsi,
            Imo = arrival.Imo,
            ShipType = arrival.ShipType,
            Flag = arrival.Flag,
            OriginPort = arrival.OriginPort,
            EstimatedTimeOfArrival = arrival.EstimatedTimeOfArrival,
            DistanceToPort = arrival.DistanceToPort
        };
    }

    public static List<PortArrivalResponseDto> ToResponseDtoList(this List<PortArrival> arrivals)
    {
        return arrivals.Select(a => a.ToResponseDto()).ToList();
    }
}

