namespace Atmosphere.Application.Readings.Commands;

using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;

using MediatR;

public class CreateReadingHandler : IRequestHandler<CreateReading, Reading>
{
    private readonly IReadingRepository _readingRepository;
    
    public CreateReadingHandler(IReadingRepository readingRepository)
    {
        _readingRepository = readingRepository;
    }
    
    public async Task<Reading> Handle(CreateReading request, CancellationToken cancellationToken)
    {
        var reading = Reading.Create(request.DeviceId, request.Value, request.Timestamp, request.Type);
        
        await _readingRepository.AddAsync(reading);
        
        return reading;
    }
}