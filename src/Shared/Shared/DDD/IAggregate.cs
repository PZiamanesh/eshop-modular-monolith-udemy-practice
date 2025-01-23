namespace Shared.DDD;

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] CleanDomainEvents();
}

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}