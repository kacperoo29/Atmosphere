namespace Atmosphere.Application.DTO.Mapping;

using Atmosphere.Application.Extensions;
using Atmosphere.Core.Models;
using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap(typeof(Core.PagedList<>), typeof(Core.PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
        CreateMap<Core.Models.Device, DeviceDto>().ConvertUsing<DeviceConverter>();
        CreateMap<Core.Models.Reading, ReadingDto>().ConvertUsing<ReadingConverter>();
        CreateMap<Core.Models.BaseUser, BaseUserDto>();
        CreateMap<Core.Models.User, BaseUserDto>();
        CreateMap<Core.Models.Device, BaseUserDto>();
        CreateMap<Core.Models.User, UserDto>();
        CreateMap<ConfigurationEntry, Dictionary<string, object?>>()
            .ConvertUsing(src => new Dictionary<string, object?> { { src.Key, src.Value } });

        CreateMap<IEnumerable<ConfigurationEntry>, Dictionary<string, object?>>()
            .ConvertUsing(src => src.ToDictionary(x => x.Key, x => x.Value));

        CreateMap<Core.Validation.ValidationRule, ValidationRuleDto>()
            .ForMember(
                dest => dest.Condition,
                opt => opt.MapFrom(src => src.Condition.ToStringReadable())
            );
    }
}
