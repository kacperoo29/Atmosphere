using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetConfigurationEntry : IRequest<object?>
{
    [Required]
    public string Key { get; init; }

    public GetConfigurationEntry()
    {
        Key = string.Empty;
    }
}