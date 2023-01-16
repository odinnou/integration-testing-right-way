#nullable disable warnings

namespace Service.DrivingAdapters.RestAdapters.Dtos;

public class PandaDto : InsertPandaDto
{
    public Guid Id { get; set; }
    public string? LastKnownAddress { get; set; }
}
