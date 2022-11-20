namespace Atmosphere.Application.DTO.Mapping;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Core.Models.Device, DeviceDto>();
        CreateMap<Core.Models.Reading, ReadingDto>();
        CreateMap<Core.Models.BaseUser, BaseUserDto>();
        CreateMap<Core.Models.User, BaseUserDto>();
        CreateMap<Core.Models.Device, BaseUserDto>();
    }
}