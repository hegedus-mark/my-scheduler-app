namespace Domain.Shared.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
