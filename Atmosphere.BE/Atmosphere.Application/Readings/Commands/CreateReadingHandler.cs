namespace Atmosphere.Application.Readings.Commands;

using System.ComponentModel.DataAnnotations;

using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MediatR;

public class CreateReadingHandler : IRequestHandler<CreateReading, Reading>
{
    private readonly IReadingRepository _readingRepository;
    private readonly INotificationService _notificationService;

    public CreateReadingHandler(IReadingRepository readingRepository,
        INotificationService notificationService)
    {
        _readingRepository = readingRepository;
        _notificationService = notificationService;
    }

    public async Task<Reading> Handle(CreateReading request, CancellationToken cancellationToken)
    {
        var reading = Reading.Create(request.DeviceId, request.Value, request.Timestamp, request.Type);        
        await _readingRepository.AddAsync(reading);
        
        return reading;
    }
}