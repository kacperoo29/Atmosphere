using Atmosphere.Application.DTO;
using Atmosphere.Core;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetPagedReadingsByDateHandler
    : IRequestHandler<GetPagedReadingsByDate, PagedList<ReadingDto>>
{
    private readonly IReadingRepository _readingRepository;
    private readonly IMapper _mapper;

    public GetPagedReadingsByDateHandler(IReadingRepository readingRepository, IMapper mapper)
    {
        _readingRepository = readingRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<ReadingDto>> Handle(
        GetPagedReadingsByDate request,
        CancellationToken cancellationToken
    )
    {
        var readings = await _readingRepository.GetAllPagedReadings(
            request.PageNumber,
            request.PageSize,
            x => x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate
        );

        return _mapper.Map<PagedList<ReadingDto>>(readings);
    }
}
