using AutoMapper;
using Domain.Models;

namespace Service.DrivingAdapters.RestAdapters.Dtos.Mappings;

public class PandaMappingProfile : Profile
{
    public PandaMappingProfile()
    {
        CreateMap<Panda, PandaDto>();
        CreateMap<InsertPandaDto, Panda>()
            .ForMember(dest => dest.LastKnownAddress, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
