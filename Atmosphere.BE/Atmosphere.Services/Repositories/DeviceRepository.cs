using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MongoDB.Driver;

namespace Atmosphere.Services.Repositories;

public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
{
    public DeviceRepository(IMongoCollection<Device> collection)
        : base(collection)
    {
    }
}