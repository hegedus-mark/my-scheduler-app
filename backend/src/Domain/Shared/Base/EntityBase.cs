using Domain.Shared.Interfaces;

namespace Domain.Shared.Base;

/// <summary>
///     Base class for entities with unique identifiers. Implements identity and equality comparison
///     based on the entity's ID rather than instance reference.
/// </summary>
public abstract class EntityBase : IHasId
{
    /// <summary>
    ///     Initializes a new instance of the EntityBase class with an optional ID.
    /// </summary>
    /// <param name="id">Optional unique identifier. If not provided, a new GUID will be generated.</param>
    /// <remarks>
    ///     Use the optional ID parameter when reconstructing an existing entity (e.g., from a database).
    ///     For new entities, omit the ID parameter to have a new GUID automatically generated.
    /// </remarks>
    protected EntityBase(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
    }

    /// <summary>
    ///     Gets the unique identifier for this entity.
    /// </summary>
    /// <remarks>
    ///     The ID is immutable and set during construction. It either comes from:
    ///     - A provided ID (for existing entities)
    ///     - A newly generated GUID (for new entities)
    /// </remarks>
    public Guid Id { get; }

    /// <summary>
    ///     Determines whether this entity is equal to another object based on ID comparison.
    /// </summary>
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

    /// <summary>
    ///     Gets a hash code for this entity based on its ID.
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
