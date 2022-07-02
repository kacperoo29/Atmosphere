using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetConfigurationEntry : IRequest<object?>
{
    public string? Key { get; init; }
}