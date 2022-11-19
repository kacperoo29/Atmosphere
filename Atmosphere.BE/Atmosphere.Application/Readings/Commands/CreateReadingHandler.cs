using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Readings.Commands;

public class CreateReadingHandler : IRequestHandler<CreateReading, ReadingDto>
{
    private readonly INotificationService _notificationService;
    private readonly IReadingRepository _readingRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public CreateReadingHandler(
        IReadingRepository readingRepository,
        INotificationService notificationService,
        IUserService userRepository,
        IMapper mapper
    )
    {
        _readingRepository = readingRepository;
        _notificationService = notificationService;
        _userService = userRepository;
        _mapper = mapper;
    }

    public async Task<ReadingDto> Handle(CreateReading request, CancellationToken cancellationToken)
    {
        var device = await _userService.GetCurrentAsync() as Device;
        if (device == null)
        {
            throw new UnauthorizedAccessException();
        }

        var reading = Reading.Create(
            device.Id,
            request.SensorIdentifier,
            request.Value,
            request.Timestamp,
            request.Type
        );

        await _readingRepository.AddAsync(reading);

        return this._mapper.Map<ReadingDto>(reading);
    }
}
