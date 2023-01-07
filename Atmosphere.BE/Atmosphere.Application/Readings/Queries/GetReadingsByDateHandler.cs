using Atmosphere.Application.DTO;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetReadingsByDateHandler : IRequestHandler<GetReadingsByDate, List<ReadingDto>>
{
    private readonly IReadingRepository _readingRepository;
    private readonly IMapper _mapper;

    public GetReadingsByDateHandler(IReadingRepository readingRepository, IMapper mapper)
    {
        _readingRepository = readingRepository;
        _mapper = mapper;
    }

    public async Task<List<ReadingDto>> Handle(
        GetReadingsByDate request,
        CancellationToken cancellationToken
    )
    {
        var readings = await _readingRepository.GetAllAsync(
            x => x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate
        );

        return _mapper.Map<List<ReadingDto>>(readings);
    }
}
