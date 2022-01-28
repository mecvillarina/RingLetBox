using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<AuditTokenCreation> AuditTokenCreations { get; set; }
        DbSet<HolderStandardToken> HolderStandardTokens { get; set; }
        DbSet<StandardToken> StandardTokens { get; set; }
        DbSet<LockTransaction> LockTransactions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}