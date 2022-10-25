namespace Atmosphere.Application.Readings.Commands;

using System.Threading;
using System.Threading.Tasks;

using Atmoshpere.Application.Services;
using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MediatR;

public class CreateReadingHandler : IRequestHandler<CreateReading, Reading>
{
    private readonly IReadingRepository _readingRepository;
    private readonly INotificationService _notificationService;
    private readonly IUserService _userService;

    public CreateReadingHandler(IReadingRepository readingRepository,
        INotificationService notificationService,
        IUserService userRepository)
    {
        _readingRepository = readingRepository;
        _notificationService = notificationService;
        _userService = userRepository;
    }

    public async Task<Reading> Handle(CreateReading request, CancellationToken cancellationToken)
    {
        var device = await this._userService.GetCurrent() as Device;
        if (device == null)
        {
            throw new UnauthorizedAccessException();
        }

        var reading = Reading.Create(device.Id, request.Value, request.Timestamp, request.Type);
        await this._readingRepository.AddAsync(reading);

        return reading;
    }
}