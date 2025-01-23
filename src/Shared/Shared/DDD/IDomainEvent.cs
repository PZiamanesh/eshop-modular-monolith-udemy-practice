using MediatR;

namespace Shared.DDD;

public interface IDomainEvent : INotification
{
    Guid EventID => Guid.NewGuid();
    DateTime OccuredOn => DateTime.Now;
    string EventType => GetType().AssemblyQualifiedName!;
}
