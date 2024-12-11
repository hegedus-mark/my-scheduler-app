namespace Scheduler.Core.Models;

public abstract class EntityBase
{
    public Guid Id { get; private set; }

    protected EntityBase()
        : this(Guid.NewGuid()) { }

    protected EntityBase(Guid id)
    {
        Id = id;
    }
}
