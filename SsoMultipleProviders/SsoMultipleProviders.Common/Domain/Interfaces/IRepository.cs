using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Domain.Interfaces
{
    public interface IRepository<TEntity, TId>
        where TEntity : IEntity<TId>
        where TId : struct
    {
        // Returns the Id
        Task<TId> Create(TEntity entity);

        Task<TEntity?> Get(TId id, CancellationToken cancellationToken = default);

        Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task Update(TEntity entity);

        // Delete an entity by its Id
        Task Delete(TId id);
        Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        void Delete(TEntity entity);
    }
}
