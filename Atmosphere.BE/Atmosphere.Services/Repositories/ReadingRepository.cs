using Atmosphere.Core.Enums;
using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MongoDB.Driver;

namespace Atmosphere.Services.Repositories;

public class ReadingRepository : BaseRepository<Reading>, IReadingRepository
{
    public ReadingRepository(IMongoCollection<Reading> collection)
        : base(collection)
    {
    }

    public async Task<IEnumerable<Reading>> GetLatestReadings(Guid deviceId, int count)
    {
        return await _collection.Find(r => r.DeviceId == deviceId)
            .SortByDescending(r => r.CreatedAt)
            .Limit(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reading>> GetLAtestReadingsByType(Guid deviceId, int count, ReadingType type)
    {
        return await _collection.Find(r => r.DeviceId == deviceId && r.Type == type)
            .SortByDescending(r => r.CreatedAt)
            .Limit(count)
            .ToListAsync();
    }

    public async Task<Reading> GetPrevious(Guid deviceId, ReadingType type)
    {
        return await _collection.Find(r => r.DeviceId == deviceId && r.Type == type)
            .SortByDescending(r => r.CreatedAt)
            .Limit(1)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Reading>> GetReadings(Guid deviceId, DateTime start, DateTime end)
    {
        return await _collection.Find(r => r.DeviceId == deviceId && r.CreatedAt >= start && r.CreatedAt <= end)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reading>> GetReadingsByType(Guid deviceId, DateTime start, DateTime end,
        ReadingType type)
    {
        return await _collection.Find(r =>
                r.DeviceId == deviceId && r.CreatedAt >= start && r.CreatedAt <= end && r.Type == type)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}