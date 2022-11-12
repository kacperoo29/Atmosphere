using System.Text;
using System.Text.Json;
using Atmoshpere.Application.Exceptions;
using Atmoshpere.Services.Consts;
using Atmosphere.Application.Auth.Commands;
using Atmosphere.Application.Readings.Commands;
using MediatR;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace Atmoshpere.API.Controllers;

public class MqttController
{
    private readonly IMediator _mediator;

    public MqttController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ValidateConnection(ValidatingConnectionEventArgs args)
    {
        try
        {
            await this._mediator.Send(
                new Authenticate { Identifier = args.UserName, Key = args.Password }
            );
        }
        catch (AuthException)
        {
            args.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
        }
    }

    public async Task InterceptPublish(InterceptingPublishEventArgs args)
    {
        switch (args.ApplicationMessage.Topic)
        {
            case MqttConsts.ReadingsTopic:

                var json = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
                var reading = JsonSerializer.Deserialize<CreateReading>(
                    json,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                );

                if (reading == null)
                {
                    throw new Exception("Invalid create reading message");
                }

                break;
            case MqttConsts.ConfigTopic:
                break;

            default:
                throw new Exception("Unknown topic");
        }
    }
}
