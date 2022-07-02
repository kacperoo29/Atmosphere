using Atmosphere.Core.Models;

namespace Atmosphere.Core.Repositories;

public interface IReadingRepository : IBaseRepository<Reading>
{
    public Task<IEnumerable<Reading>> GetLatestReadings(Guid deviceId, int count);

    public Task<IEnumerable<Reading>> GetReadings(Guid deviceId, DateTime start, DateTime end);

    public Task<IEnumerable<Reading>> GetLAtestReadingsByType(Guid deviceId, int count, ReadingType type);

    public Task<IEnumerable<Reading>> GetReadingsByType(Guid deviceId, DateTime start, DateTime end, ReadingType type);

    public Task<Reading> GetPrevious(Guid deviceId, ReadingType type);
}