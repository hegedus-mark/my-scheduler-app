using Application.Shared.Contracts;
using Infrastructure.Shared.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Shared.Persistence;

/// <summary>
///     Base implementation of the Unit of Work pattern that coordinates database operations
///     and transaction management using Entity Framework Core.
/// </summary>
/// <remarks>
///     <para>
///         The Unit of Work pattern ensures that all database operations within a business transaction
///         are treated as a single unit. This means that either all operations succeed together,
///         or they all fail together, maintaining data consistency.
///     </para>
///     <para>
///         Key responsibilities:
///         - Managing database transactions
///         - Coordinating save operations
///         - Ensuring proper resource cleanup
///     </para>
/// </remarks>
public class BaseUnitOfWork : IBaseUnitOfWork
{
    /// <summary>
    ///     The Entity Framework Core database context used for database operations.
    /// </summary>
    protected readonly AppDbContext Context;

    /// <summary>
    ///     The current active database transaction, if any.
    /// </summary>
    private IDbContextTransaction? _currentTransaction;

    /// <summary>
    ///     Flag indicating whether the unit of work has been disposed.
    /// </summary>
    protected bool Disposed;

    /// <summary>
    ///     Initializes a new instance of the BaseUnitOfWork class.
    /// </summary>
    /// <param name="context">The database context to use for operations.</param>
    public BaseUnitOfWork(AppDbContext context)
    {
        Context = context;
    }

    /// <summary>
    ///     Gets a value indicating whether there is an active transaction.
    /// </summary>
    /// <value>True if there is an active transaction; otherwise, false.</value>
    public bool HasActiveTransaction => _currentTransaction != null;

    /// <summary>
    ///     Asynchronously saves all changes made within the unit of work to the database.
    /// </summary>
    /// <returns>
    ///     The number of state entries written to the database.
    /// </returns>
    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    /// <summary>
    ///     Releases all resources used by the unit of work.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Asynchronously begins a new database transaction.
    /// </summary>
    /// <returns>
    ///     The newly created transaction if no transaction exists;
    ///     null if a transaction is already in progress.
    /// </returns>
    /// <remarks>
    ///     This method ensures that only one transaction is active at a time.
    ///     If called while a transaction is already in progress, it returns null
    ///     instead of creating a new transaction.
    /// </remarks>
    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
            return null;

        _currentTransaction = await Context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    /// <summary>
    ///     Asynchronously commits the current transaction and saves all changes.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method:
    ///         1. Saves all pending changes to the database
    ///         2. Commits the transaction if one exists
    ///         3. Rolls back the transaction if any error occurs
    ///         4. Ensures proper cleanup of transaction resources
    ///     </para>
    /// </remarks>
    /// <exception cref="Exception">
    ///     Rethrows any exception that occurs during the save or commit operation,
    ///     after attempting to rollback the transaction.
    /// </exception>
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

    /// <summary>
    ///     Asynchronously rolls back the current transaction, if one exists.
    /// </summary>
    /// <remarks>
    ///     This method safely handles the rollback even if no transaction exists,
    ///     ensuring proper cleanup of transaction resources.
    /// </remarks>
    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the unit of work and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     True to release both managed and unmanaged resources; false to release only unmanaged
    ///     resources.
    /// </param>
    /// <remarks>
    ///     This method follows the dispose pattern to ensure proper cleanup of resources:
    ///     - Disposes the current transaction if one exists
    ///     - Disposes the database context
    ///     - Sets the disposed flag to prevent multiple disposals
    /// </remarks>
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
