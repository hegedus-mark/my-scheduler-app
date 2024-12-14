namespace Scheduler.Domain.Models.Base;

/// <summary>
///     Base class for entities with unique identifiers.
/// </summary>
public abstract class EntityBase
{
    protected EntityBase(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
    }

    public Guid Id { get; private set; }
}
