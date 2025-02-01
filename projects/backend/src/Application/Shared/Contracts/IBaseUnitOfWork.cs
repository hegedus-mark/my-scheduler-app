using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Shared.Contracts;

public interface IBaseUnitOfWork : IDisposable
{
    bool HasActiveTransaction { get; }
    Task<IDbContextTransaction?> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync();
}
