namespace Atmosphere.Application.DTO.Mapping;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Models.Device, DeviceDto>();
        CreateMap<Core.Models.Reading, ReadingDto>();
    }
}