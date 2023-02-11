using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;

namespace Atmosphere.Application.DTO.Mapping;

public class ReadingConverter : ITypeConverter<Reading, ReadingDto>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public ReadingConverter(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public ReadingDto Convert(Reading source, ReadingDto destination, ResolutionContext context)
    {
        var device = _deviceRepository.GetByIdAsync(source.DeviceId).Result;

        return new ReadingDto
        {
            Id = source.Id,
            Device = _mapper.Map<DeviceDto>(device),
            Value = source.Value,
            Unit = source.Unit,
            Timestamp = source.Timestamp,
            Type = source.Type
        };
    }
}
