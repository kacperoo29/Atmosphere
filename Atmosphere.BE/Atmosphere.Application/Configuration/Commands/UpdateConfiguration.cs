using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class UpdateConfiguration : IRequest<object?>
{
    [Required]
    public string Key { get; init; }
    
    [Required]
    public object? Value { get; init; }

    public UpdateConfiguration()
    {
        Key = string.Empty;
        Value = null;
    }
}