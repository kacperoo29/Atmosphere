using Atmosphere.Core.Models;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateConfiguration : IRequest<object?>
{
    public string? Key { get; init; }
    public object? Value { get; init; }
}