using MediatR;

namespace Application.Abstract;

public interface IIntegrationEvent : INotification
{
    Guid Id { get; init; }
}