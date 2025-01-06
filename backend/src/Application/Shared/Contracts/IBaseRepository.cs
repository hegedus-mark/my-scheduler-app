using Domain.Shared.Base;

namespace Application.Shared.Contracts;

public interface IBaseRepository<TDomain>
    where TDomain : EntityBase
{
    Task<TDomain?> GetByIdAsync(Guid id, bool asNoTracking = false);
    Task<IReadOnlyList<TDomain>> GetAllAsync(bool asNoTracking = false);
    Task AddAsync(TDomain domain);
    Task AddRangeAsync(IEnumerable<TDomain> domains);
    void Update(TDomain domain);
    void UpdateRange(IEnumerable<TDomain> domains);
    Task RemoveAsync(TDomain domain);
    Task RemoveRangeAsync(IEnumerable<TDomain> domains);
}
