using Atmosphere.Application.DTO;
using MediatR;

namespace Atmosphere.Application.Devices.Queries;

public class GetAllDevices : IRequest<List<DeviceDto>> { }
