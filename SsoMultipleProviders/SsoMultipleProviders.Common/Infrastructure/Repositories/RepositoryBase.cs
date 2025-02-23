using Microsoft.EntityFrameworkCore;
using SsoMultipleProviders.Common.Domain;
using SsoMultipleProviders.Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : struct
    {
        private readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }
        public virtual async Task<TId> Create(TEntity entity)
        {
            _context.Add(entity);
            return await Task.FromResult(entity.Id);
        }

        public virtual async Task Delete(TId id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _context.Remove(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public virtual async Task<TEntity?> Get(TId id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
            return entity;
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<TEntity>().AsQueryable().AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            var entities = await query.ToListAsync();
            return new ReadOnlyCollection<TEntity>(entities);
        }

        public virtual async Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<TEntity>().AsQueryable().AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            var entity = await query.FirstOrDefaultAsync();
            return entity;
        }

        public virtual async Task Update(TEntity entity)
        {
            _context.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (predicate == null)
            {
                return await _context.Set<TEntity>().CountAsync();
            }
            return await _context.Set<TEntity>().CountAsync(predicate);
        }
    }
}
