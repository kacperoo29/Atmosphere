using MediatR;

namespace Atmosphere.Application.Configuration.Commands;

public class SetPollingRate : IRequest
{
    public int PollingRate { get; set; }
}