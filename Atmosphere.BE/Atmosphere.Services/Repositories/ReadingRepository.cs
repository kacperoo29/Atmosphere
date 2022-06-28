using Atmosphere.Core.Models;
using Atmosphere.Core.Repositories;
using MongoDB.Driver;

namespace Atmosphere.Services.Repositories;

public class ReadingRepository : BaseRepository<Reading>, IReadingRepository
{
    public ReadingRepository(IMongoCollection<Reading> context)
        : base(context)
    {
    }
}