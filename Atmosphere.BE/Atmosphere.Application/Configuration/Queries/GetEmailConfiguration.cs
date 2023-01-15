using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetEmailConfiguration : IRequest<EmailConfiguration>
{
}