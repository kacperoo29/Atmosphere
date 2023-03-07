using System.Linq.Expressions;
using Atmosphere.Application.DTO;
using Atmosphere.Core;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetPagedReadingsByDeviceHandler
    : IRequestHandler<GetPagedReadingsByDevice, PagedList<ReadingDto>>
{
    private readonly IReadingRepository _readingRepository;
    private readonly IMapper _mapper;

    public GetPagedReadingsByDeviceHandler(IReadingRepository readingRepository, IMapper mapper)
    {
        _readingRepository = readingRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<ReadingDto>> Handle(
        GetPagedReadingsByDevice request,
        CancellationToken cancellationToken
    )
    {
        var param = Expression.Parameter(typeof(Reading), "x");
        var expr = Expression.Equal(
            Expression.Property(param, nameof(Reading.DeviceId)),
            Expression.Constant(request.DeviceId)
        );

        if (request.StartDate.HasValue)
        {
            expr = Expression.AndAlso(
                expr,
                Expression.GreaterThanOrEqual(
                    Expression.Property(param, nameof(Reading.CreatedAt)),
                    Expression.Constant(request.StartDate.Value)
                )
            );
        }

        if (request.EndDate.HasValue)
        {
            expr = Expression.AndAlso(
                expr,
                Expression.LessThanOrEqual(
                    Expression.Property(param, nameof(Reading.CreatedAt)),
                    Expression.Constant(request.EndDate.Value)
                )
            );
        }

        var lambda = Expression.Lambda<Func<Reading, bool>>(expr, param);
        var readings = await _readingRepository.GetAllPagedReadings(
            request.PageNumber,
            request.PageSize,
            lambda
        );

        return _mapper.Map<PagedList<ReadingDto>>(readings);
    }
}