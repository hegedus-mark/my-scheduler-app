using SharedKernel.Domain.Interfaces;

namespace SharedKernel.Domain.Base;

/// <summary>
///     Base class for entities with unique identifiers.
/// </summary>
public abstract class EntityBase : IHasId
{
    protected EntityBase(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
    }

    public Guid Id { get; }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not EntityBase)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var other = (EntityBase)obj;
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
