#nullable disable warnings

using System.ComponentModel.DataAnnotations;

namespace Service.DrivingAdapters.RestAdapters.Dtos;

public class InsertPandaDto
{
    [Required]
    public string Name { get; set; }

    [Range(-90.0, 90.0)]
    public decimal Latitude { get; set; }

    [Range(-180.0, 180.0)]
    public decimal Longitude { get; set; }
}
