namespace Atmosphere.Application.DTO.Mapping;

using Atmosphere.Core.Models;
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
        CreateMap<ConfigurationEntry, Dictionary<string, object?>>()
            .ConvertUsing(src => new Dictionary<string, object?> { { src.Key, src.Value } });

        CreateMap<IEnumerable<ConfigurationEntry>, Dictionary<string, object?>>()
            .ConvertUsing(src => src.ToDictionary(x => x.Key, x => x.Value));

        CreateMap<Core.Models.NotificationSettings, NotificationSettingsDto>();
    }
}
