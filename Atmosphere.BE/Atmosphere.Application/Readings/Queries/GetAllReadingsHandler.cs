using Atmosphere.Application.DTO;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetAllReadingsHandler : IRequestHandler<GetAllReadings, IEnumerable<ReadingDto>>
{
    private readonly IReadingRepository _readingRepository;
    private readonly IMapper _mapper;

    public GetAllReadingsHandler(IReadingRepository readingRepository, IMapper mapper)
    {
        _readingRepository = readingRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReadingDto>> Handle(
        GetAllReadings request,
        CancellationToken cancellationToken
    )
    {
        var readings = await _readingRepository.GetAllAsync();

        return this._mapper.Map<IEnumerable<ReadingDto>>(readings);
    }
}
