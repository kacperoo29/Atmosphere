using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetAllReadingsHandler : IRequestHandler<GetAllReadings, IEnumerable<Reading>>
{
    private readonly IReadingRepository _readingRepository;

    public GetAllReadingsHandler(IReadingRepository readingRepository)
    {
        _readingRepository = readingRepository;
    }

    public async Task<IEnumerable<Reading>> Handle(GetAllReadings request, CancellationToken cancellationToken)
    {
        return await _readingRepository.GetAllAsync(r => r.DeviceId == request.DeviceId);
    }
}