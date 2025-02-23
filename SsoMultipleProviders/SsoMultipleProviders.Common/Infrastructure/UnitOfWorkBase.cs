using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SsoMultipleProviders.Common.Domain;
using SsoMultipleProviders.Common.Domain.Interfaces;
using System.Security.Cryptography;

namespace SsoMultipleProviders.Common.Infrastructure
{
    public abstract class UnitOfWorkBase<TDbContext> : IUnitOfWorkBase where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;

        public UnitOfWorkBase(
            TDbContext identityProviderDbContext,
            IServiceProvider serviceProvider
        )
        {
            _dbContext = identityProviderDbContext;
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException("serviceProvider");
            _repositories = new Dictionary<Type, object>();
        }

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task Rollback(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            throw new InvalidOperationException("Cannot Rollback transaction if transaction is null");
        }

        public async Task Commit(CancellationToken cancellationToken = default)
        {

            if (_transaction != null)
            {
                try
                {
                    await _transaction.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await _transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                return;
            }
            throw new InvalidOperationException("Cannot commit transaction if transaction is null");
        }

        public abstract Task SaveChangesAsync(CancellationToken cancellationToken);

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            var key = typeof(TRepository);
            if (!_repositories.ContainsKey(key))
            {

                var repository = _serviceProvider.GetService<TRepository>();
                if (repository is null)
                {
                    throw new ArgumentNullException(typeof(TRepository).Name);
                }
                if (repository.GetType().GetInterfaces().Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IRepository<,>)))
                {
                    _repositories[key] = repository;
                }
                else
                {
                    throw new InvalidOperationException(typeof(TRepository).Name + " does not implement IRepository");
                }
                return repository;
            }
            return (TRepository)_repositories[key];
        }
    }
}
