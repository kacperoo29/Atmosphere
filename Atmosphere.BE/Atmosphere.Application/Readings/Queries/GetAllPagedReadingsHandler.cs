using Atmosphere.Application.DTO;
using Atmosphere.Core;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetAllPagedReadingsHandler : IRequestHandler<GetAllPagedReadings, PagedList<ReadingDto>>
{
    private readonly IReadingRepository _readingRepository;
    private readonly IMapper _mapper;

    public GetAllPagedReadingsHandler(IReadingRepository readingRepository, IMapper mapper)
    {
        _readingRepository = readingRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<ReadingDto>> Handle(GetAllPagedReadings request, CancellationToken cancellationToken)
    {
        var readings = await _readingRepository.GetAllPagedReadings(request.PageNumber, request.PageSize);
        return _mapper.Map<PagedList<ReadingDto>>(readings);
    }
}