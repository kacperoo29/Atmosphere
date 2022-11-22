using Atmosphere.Application.DTO;
using Atmosphere.Application.Services;
using AutoMapper;
using MediatR;

namespace Atmosphere.Application.Configuration.Queries;

public class GetNotificationSettingsHandler : IRequestHandler<GetNotificationSettings, NotificationSettingsDto>
{
    private readonly IConfigService _configService;
    private readonly IMapper _mapper;

    public GetNotificationSettingsHandler(IConfigService configService, IMapper mapper)
    {
        _configService = configService;
        _mapper = mapper;
    }

    public async Task<NotificationSettingsDto> Handle(GetNotificationSettings request, CancellationToken cancellationToken)
    {
        var settings = await _configService.GetNotificationSettingsAsync(cancellationToken);

        return _mapper.Map<NotificationSettingsDto>(settings);
    }
}