using Application.Shared.Contracts;
using Domain.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Persistence;

internal abstract class BaseRepository<TDomain, TEntity> : IBaseRepository<TDomain>
    where TEntity : class, IEntity
    where TDomain : EntityBase
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TDomain domain)
    {
        var entity = MapToEntity(domain);
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TDomain> domains)
    {
        var entities = domains.Select(MapToEntity);
        await DbSet.AddRangeAsync(entities);
    }

    public async Task RemoveAsync(TDomain domain)
    {
        var entity = await DbSet.FindAsync(domain.Id);
        if (entity != null)
            DbSet.Remove(entity);
    }

    public async Task RemoveRangeAsync(IEnumerable<TDomain> domains)
    {
        var ids = domains.Select(d => d.Id).ToList();
        var entities = await DbSet.Where(e => ids.Contains(e.Id)).ToListAsync();
        DbSet.RemoveRange(entities);
    }

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

    public async Task<IReadOnlyList<TDomain>> GetAllAsync(bool asNoTracking = false)
    {
        var query = asNoTracking ? DbSet.AsNoTracking() : DbSet;
        var entities = await query.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

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

    protected abstract TDomain MapToDomain(TEntity entity);
    protected abstract TEntity MapToEntity(TDomain domain);
    protected abstract void MapToExistingEntity(TDomain domain, TEntity entity);
}
