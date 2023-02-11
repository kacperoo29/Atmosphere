namespace Atmosphere.Application.Readings.Queries;

using System.ComponentModel.DataAnnotations;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

public class GetReadingsByDeviceHandler : IRequestHandler<GetReadingsByDevice, List<ReadingDto>>
{
    private readonly IReadingRepository _readingRepository;
    private readonly IMapper _mapper;

    public GetReadingsByDeviceHandler(IReadingRepository readingRepository, IMapper mapper)
    {
        _readingRepository = readingRepository;
        _mapper = mapper;
    }

    public async Task<List<ReadingDto>> Handle(
        GetReadingsByDevice request,
        CancellationToken cancellationToken
    )
    {
        var readings = await _readingRepository.GetAllAsync(x => x.DeviceId == request.DeviceId);

        return _mapper.Map<List<ReadingDto>>(readings);
    }
}
