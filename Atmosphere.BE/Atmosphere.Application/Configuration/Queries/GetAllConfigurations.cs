using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetAllConfigurations : IRequest<Dictionary<string, object?>>
{
}