namespace Scheduler.Application.Interfaces.Mapping;

public interface IMapper<TDomain, TEntity>
{
    TEntity ToEntity(TDomain domain);
    TDomain ToDomain(TEntity entity);
}
