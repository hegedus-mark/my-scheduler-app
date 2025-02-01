using Domain.Shared.Interfaces;

namespace Domain.Shared.Base;

/// <summary>
///     Base class for aggregate roots in the domain model. An aggregate root is the main entity in a cluster
///     of related domain objects that should be treated as a single unit for data changes.
/// </summary>
/// <remarks>
///     <para>
///         In Domain-Driven Design, an aggregate is a cluster of domain objects that can be treated as a single unit.
///         An aggregate root is the entry point to this cluster and ensures the consistency of changes to the objects
///         within the aggregate by defining invariants (business rules).
///     </para>
///     <para>
///         Key characteristics of an aggregate root:
///         - It's responsible for enforcing invariants across entity boundaries within the aggregate
///         - All external references must only point to the aggregate root
///         - Child entities can only be changed through the aggregate root
///         - When an aggregate root is deleted, all entities within its boundary must also be deleted
///     </para>
///     <para>
///         This implementation includes domain event handling capabilities, allowing the aggregate root to:
///         - Record domain events during business operations
///         - Maintain a collection of pending domain events
///         - Clear processed events
///     </para>
/// </remarks>
public class AggregateRoot : EntityBase, IHasDomainEvents
{
    /// <summary>
    ///     Collection of domain events that have occurred but haven't been dispatched yet.
    /// </summary>
    /// <remarks>
    ///     Events are typically dispatched and cleared by the infrastructure layer after
    ///     completing the business transaction.
    /// </remarks>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    ///     Initializes a new instance of the AggregateRoot class.
    /// </summary>
    /// <param name="id">Optional unique identifier. If not provided, a new GUID will be generated.</param>
    protected AggregateRoot(Guid? id = null)
        : base(id) { }

    /// <summary>
    ///     Gets the collection of undispatched domain events.
    /// </summary>
    /// <remarks>
    ///     Returns a read-only view of the domain events to prevent external modifications.
    ///     Events should only be added through the protected AddDomainEvent method.
    /// </remarks>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Clears all undispatched domain events.
    /// </summary>
    /// <remarks>
    ///     This method should be called after all events have been dispatched, typically
    ///     by the infrastructure layer after committing the unit of work.
    /// </remarks>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    ///     Adds a new domain event to the collection of undispatched events.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    /// <remarks>
    ///     Protected access ensures that only the aggregate root and its derived classes
    ///     can add domain events, maintaining encapsulation of the event raising process.
    /// </remarks>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
