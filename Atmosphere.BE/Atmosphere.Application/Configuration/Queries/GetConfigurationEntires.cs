using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetConfigurationEntries : IRequest<IEnumerable<ConfigurationEntry>>
{
    public string[] Keys { get; init; }
}