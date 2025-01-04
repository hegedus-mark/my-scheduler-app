using Microsoft.EntityFrameworkCore;
using SharedKernel.Domain.Base;
using SharedKernel.Persistence;

namespace Infrastructure.Persistence;

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

    public async Task<TDomain?> GetByIdAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IReadOnlyList<TDomain>> GetAllAsync()
    {
        var entities = await DbSet.ToListAsync();
        return entities.Select(MapToDomain).ToList();
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

    public async Task UpdateAsync(TDomain domain)
    {
        var entity = MapToEntity(domain);
        DbSet.Update(entity);
        await Task.CompletedTask;
    }

    public async Task UpdateRangeAsync(IEnumerable<TDomain> domains)
    {
        var entities = domains.Select(MapToEntity);
        DbSet.UpdateRange(entities);
        await Task.CompletedTask;
    }

    public async Task RemoveAsync(TDomain domain)
    {
        var entity = MapToEntity(domain);
        DbSet.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task RemoveRangeAsync(IEnumerable<TDomain> domains)
    {
        var entities = domains.Select(MapToEntity);
        DbSet.RemoveRange(entities);
        await Task.CompletedTask;
    }

    protected abstract TDomain MapToDomain(TEntity entity);
    protected abstract TEntity MapToEntity(TDomain domain);
}
