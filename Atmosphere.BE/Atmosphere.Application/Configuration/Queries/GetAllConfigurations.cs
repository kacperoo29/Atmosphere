namespace Atmosphere.Application.Configuration.Queries;

using Atmosphere.Core.Models;

using MediatR;

public class GetAllConfigurations : IRequest<IEnumerable<ConfigurationEntry>>
{
}