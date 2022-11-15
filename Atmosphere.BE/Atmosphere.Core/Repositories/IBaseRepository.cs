using System.Linq.Expressions;
using Atmosphere.Core.Models;

namespace Atmosphere.Core.Repositories;

public interface IBaseRepository<T> where T : BaseModel
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default);
}