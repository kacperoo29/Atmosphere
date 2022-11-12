using Atmoshpere.Application.Services;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Readings.Commands;

public class CreateReadingHandler : IRequestHandler<CreateReading, Reading>
{
    private readonly INotificationService _notificationService;
    private readonly IReadingRepository _readingRepository;
    private readonly IUserService _userService;

    public CreateReadingHandler(
        IReadingRepository readingRepository,
        INotificationService notificationService,
        IUserService userRepository
    )
    {
        _readingRepository = readingRepository;
        _notificationService = notificationService;
        _userService = userRepository;
    }

    public async Task<Reading> Handle(CreateReading request, CancellationToken cancellationToken)
    {
        var device = await _userService.GetCurrent() as Device;
        if (device == null)
            throw new UnauthorizedAccessException();

        var reading = Reading.Create(
            device.Id,
            request.DeviceAddress,
            request.Value,
            DateTime.Now,
            request.Type
        );
        await _readingRepository.AddAsync(reading);

        return reading;
    }
}
