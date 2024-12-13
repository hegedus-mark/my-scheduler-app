namespace Scheduler.Core.Models;

/// <summary>
///     Base class for entities with unique identifiers.
/// </summary>
public abstract class EntityBase
{
    protected EntityBase()
        : this(Guid.NewGuid()) { }

    protected EntityBase(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
