using System.ComponentModel.DataAnnotations;
using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetConfigurationEntries : IRequest<Dictionary<string, object?>>
{
    [Required]
    public string[] Keys { get; init; }

    public GetConfigurationEntries()
    {
        Keys = new string[0];
    }
}