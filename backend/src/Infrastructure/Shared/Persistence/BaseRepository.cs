using Application.Shared.Contracts;
using Domain.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Persistence;

/// <summary>
///     Base repository implementation that provides common data access operations using Entity Framework Core.
///     Implements the repository pattern to abstract the data persistence mechanisms from the domain model.
/// </summary>
/// <typeparam name="TDomain">The domain entity type that inherits from EntityBase.</typeparam>
/// <typeparam name="TEntity">The database entity type that implements IEntity.</typeparam>
/// <remarks>
///     <para>
///         This repository implementation:
///         - Provides basic CRUD operations
///         - Handles entity tracking and change detection
///         - Supports both tracked and untracked queries
///         - Manages mapping between domain and database entities
///     </para>
/// </remarks>
internal abstract class BaseRepository<TDomain, TEntity> : IBaseRepository<TDomain>
    where TEntity : class, IEntity
    where TDomain : EntityBase
{
    /// <summary>
    ///     The Entity Framework Core database context.
    /// </summary>
    protected readonly DbContext Context;

    /// <summary>
    ///     The DbSet for the entity type being managed by this repository.
    /// </summary>
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>
    ///     Initializes a new instance of the BaseRepository class.
    /// </summary>
    /// <param name="context">The Entity Framework Core database context.</param>
    protected BaseRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    /// <summary>
    ///     Asynchronously adds a new domain entity to the repository.
    /// </summary>
    /// <param name="domain">The domain entity to add.</param>
    /// <remarks>
    ///     The entity is mapped to its database representation before being added to the context.
    ///     Changes are not saved until the unit of work commits the transaction.
    /// </remarks>
    public async Task AddAsync(TDomain domain)
    {
        var entity = MapToEntity(domain);
        await DbSet.AddAsync(entity);
    }

    /// <summary>
    ///     Asynchronously adds multiple domain entities to the repository.
    /// </summary>
    /// <param name="domains">The collection of domain entities to add.</param>
    /// <remarks>
    ///     This method is more efficient than adding entities individually when working with multiple entities.
    ///     Changes are not saved until the unit of work commits the transaction.
    /// </remarks>
    public async Task AddRangeAsync(IEnumerable<TDomain> domains)
    {
        var entities = domains.Select(MapToEntity);
        await DbSet.AddRangeAsync(entities);
    }

    /// <summary>
    ///     Asynchronously removes a domain entity from the repository.
    /// </summary>
    /// <param name="domain">The domain entity to remove.</param>
    /// <remarks>
    ///     The entity is first loaded from the database using its ID to ensure proper change tracking.
    ///     Changes are not saved until the unit of work commits the transaction.
    /// </remarks>
    public async Task RemoveAsync(TDomain domain)
    {
        var entity = await DbSet.FindAsync(domain.Id);
        if (entity != null)
            DbSet.Remove(entity);
    }

    /// <summary>
    ///     Asynchronously removes multiple domain entities from the repository.
    /// </summary>
    /// <param name="domains">The collection of domain entities to remove.</param>
    /// <remarks>
    ///     This method performs a single query to load all entities before removing them.
    ///     Changes are not saved until the unit of work commits the transaction.
    /// </remarks>
    public async Task RemoveRangeAsync(IEnumerable<TDomain> domains)
    {
        var ids = domains.Select(d => d.Id).ToList();
        var entities = await DbSet.Where(e => ids.Contains(e.Id)).ToListAsync();
        DbSet.RemoveRange(entities);
    }

    /// <summary>
    ///     Asynchronously retrieves a domain entity by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="asNoTracking">When true, the entity is not tracked by the context.</param>
    /// <returns>The domain entity if found; null otherwise.</returns>
    /// <remarks>
    ///     First checks the change tracker for an existing entity to ensure consistency,
    ///     then queries the database if no tracked entity is found.
    /// </remarks>
    public async Task<TDomain?> GetByIdAsync(Guid id, bool asNoTracking = false)
    {
        if (!asNoTracking)
        {
            var trackedEntry = Context
                .ChangeTracker.Entries<TEntity>()
                .FirstOrDefault(e => e.Entity.Id == id && e.State != EntityState.Deleted);

            if (trackedEntry != null)
                return MapToDomain(trackedEntry.Entity);
        }

        var query = asNoTracking ? DbSet.AsNoTracking() : DbSet;
        var entity = await query.FirstOrDefaultAsync(e => e.Id == id);
        return entity != null ? MapToDomain(entity) : null;
    }

    /// <summary>
    ///     Asynchronously retrieves all domain entities.
    /// </summary>
    /// <param name="asNoTracking">When true, the entities are not tracked by the context.</param>
    /// <returns>A read-only list of all domain entities.</returns>
    public async Task<IReadOnlyList<TDomain>> GetAllAsync(bool asNoTracking = false)
    {
        var query = asNoTracking ? DbSet.AsNoTracking() : DbSet;
        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    /// <summary>
    ///     Updates multiple domain entities in the repository.
    /// </summary>
    /// <param name="domains">The collection of domain entities to update.</param>
    /// <remarks>
    ///     <para>
    ///         For each entity:
    ///         - If it's tracked, updates the existing entity
    ///         - If it's not tracked, marks it for update in the context
    ///     </para>
    ///     Changes are not saved until the unit of work commits the transaction.
    /// </remarks>
    public void UpdateRange(IEnumerable<TDomain> domains)
    {
        var domainList = domains.ToList();
        var domainIds = domainList.Select(d => d.Id).ToList();

        var trackedEntities = Context
            .ChangeTracker.Entries<TEntity>()
            .Where(e => domainIds.Contains(e.Entity.Id))
            .ToDictionary(e => e.Entity.Id, e => e.Entity);

        foreach (var domain in domainList)
            if (trackedEntities.TryGetValue(domain.Id, out var trackedEntity))
                MapToExistingEntity(domain, trackedEntity);
            else
                DbSet.Update(MapToEntity(domain));
    }

    /// <summary>
    ///     Updates a single domain entity in the repository.
    /// </summary>
    /// <param name="domain">The domain entity to update.</param>
    /// <remarks>
    ///     If the entity is tracked, updates the existing entity;
    ///     otherwise, marks it for update in the context.
    ///     Changes are not saved until the unit of work commits the transaction.
    /// </remarks>
    public void Update(TDomain domain)
    {
        var trackedEntry = Context
            .ChangeTracker.Entries<TEntity>()
            .FirstOrDefault(e => e.Entity.Id == domain.Id);

        if (trackedEntry != null)
            MapToExistingEntity(domain, trackedEntry.Entity);
        else
            DbSet.Update(MapToEntity(domain));
    }

    /// <summary>
    ///     Maps a database entity to its domain representation.
    /// </summary>
    /// <param name="entity">The database entity to map.</param>
    /// <returns>The corresponding domain entity.</returns>
    protected abstract TDomain MapToDomain(TEntity entity);

    /// <summary>
    ///     Maps a domain entity to its database representation.
    /// </summary>
    /// <param name="domain">The domain entity to map.</param>
    /// <returns>The corresponding database entity.</returns>
    protected abstract TEntity MapToEntity(TDomain domain);

    /// <summary>
    ///     Updates an existing database entity with values from a domain entity.
    /// </summary>
    /// <param name="domain">The domain entity containing the updated values.</param>
    /// <param name="entity">The database entity to update.</param>
    /// <remarks>
    ///     This method should only update the properties that can change,
    ///     preserving any database-specific or immutable properties.
    /// </remarks>
    protected abstract void MapToExistingEntity(TDomain domain, TEntity entity);
}
