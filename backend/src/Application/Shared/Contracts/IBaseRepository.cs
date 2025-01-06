using Domain.Shared.Base;

namespace Application.Shared.Contracts;

public interface IBaseRepository<TDomain>
    where TDomain : EntityBase
{
    Task<TDomain?> GetByIdAsync(Guid id, bool asNoTracking = false);
    Task<IReadOnlyList<TDomain>> GetAllAsync(bool asNoTracking = false);
    Task AddAsync(TDomain domain);
    Task AddRangeAsync(IEnumerable<TDomain> domains);
    Task UpdateAsync(TDomain domain);
    Task UpdateRangeAsync(IEnumerable<TDomain> domains);
    Task RemoveAsync(TDomain domain);
    Task RemoveRangeAsync(IEnumerable<TDomain> domains);
}
