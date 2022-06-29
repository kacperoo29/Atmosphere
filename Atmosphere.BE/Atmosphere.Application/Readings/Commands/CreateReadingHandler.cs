namespace Atmosphere.Application.Readings.Commands;

using System.ComponentModel.DataAnnotations;

using Atmosphere.Application.Services;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using Atmosphere.Core.Services;

using MediatR;

public class CreateReadingHandler : IRequestHandler<CreateReading, Reading>
{
    private readonly IReadingRepository _readingRepository;
    private readonly INotificationService _notificationService;
    private readonly IReadingValidationService _readingValidationService;

    public CreateReadingHandler(IReadingRepository readingRepository,
        INotificationService notificationService,
        IReadingValidationService readingValidationService)
    {
        _readingRepository = readingRepository;
        _notificationService = notificationService;
        _readingValidationService = readingValidationService;
    }

    public async Task<Reading> Handle(CreateReading request, CancellationToken cancellationToken)
    {
        var reading = Reading.Create(request.DeviceId, request.Value, request.Timestamp, request.Type);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        await foreach (var result in _readingValidationService.RunChecks(reading))
        {
            if (result != ValidationResult.Success)
            {
                validationResults.Add(result);
            }
        }

        if (validationResults.Count > 0)
        {
            await _notificationService.Notify(reading, validationResults);
        }
        
        await _readingRepository.AddAsync(reading);
        
        return reading;
    }
}