using Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Persistence;

namespace Infrastructure.Persistence;

public class UnitOfWork : IBaseUnitOfWork
{
    protected readonly AppDbContext Context;
    private IDbContextTransaction? _currentTransaction;
    protected bool Disposed;

    public UnitOfWork(AppDbContext context)
    {
        Context = context;
    }

    public bool HasActiveTransaction => _currentTransaction != null;

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            return null;

        _currentTransaction = await Context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();

            if (_currentTransaction != null)
                await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!Disposed && disposing)
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }

            Context.Dispose();
        }

        Disposed = true;
    }
}
