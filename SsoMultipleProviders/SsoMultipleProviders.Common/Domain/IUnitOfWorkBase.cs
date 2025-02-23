using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Domain
{
    public interface IUnitOfWorkBase
    {
        Task BeginTransaction(CancellationToken cancellationToken = default);
        Task Commit(CancellationToken cancellationToken = default);
        Task Rollback(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
