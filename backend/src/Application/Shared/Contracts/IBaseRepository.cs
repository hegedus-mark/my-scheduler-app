using Domain.Shared.Base;

namespace Application.Shared.Contracts;

public interface IBaseRepository<TDomain>
    where TDomain : EntityBase
{
    Task<TDomain?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<TDomain>> GetAllAsync();
    Task AddAsync(TDomain domain);
    Task AddRangeAsync(IEnumerable<TDomain> domains);
    Task UpdateAsync(TDomain domain);
    Task UpdateRangeAsync(IEnumerable<TDomain> domains);
    Task RemoveAsync(TDomain domain);
    Task RemoveRangeAsync(IEnumerable<TDomain> domains);
}
