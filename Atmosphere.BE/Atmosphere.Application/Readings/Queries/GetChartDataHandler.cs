using System.Linq.Expressions;
using Atmosphere.Application.DTO;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MediatR;

namespace Atmosphere.Application.Readings.Queries;

public class GetChartDataHandler
    : IRequestHandler<GetChartData, Dictionary<DateTime, decimal>>
{
    private readonly IReadingRepository _readingRepository;

    public GetChartDataHandler(IReadingRepository readingRepository)
    {
        _readingRepository = readingRepository;
    }

    public async Task<Dictionary<DateTime, decimal>> Handle(
        GetChartData request,
        CancellationToken cancellationToken
    )
    {
        ParameterExpression param = Expression.Parameter(typeof(Reading), "x");
        Expression expr = Expression.Equal(
            Expression.Property(param, nameof(Reading.Type)),
            Expression.Constant(request.ReadingType)
        );

        if (request.DeviceId.HasValue)
        {
            expr = Expression.AndAlso(
                expr,
                Expression.Equal(
                    Expression.Property(param, nameof(Reading.DeviceId)),
                    Expression.Constant(request.DeviceId.Value)
                )
            );
        }

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
        var readings = await _readingRepository.GetAllAsync(lambda);

        return readings
            .GroupBy(x => new { x.CreatedAt.Date, x.CreatedAt.Hour })
            .ToDictionary(
                x => x.Key.Date.AddHours(x.Key.Hour),
                x => x.Average(y => y.Value)
            );
    }
}
