using Application.Features.RegisterUser;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.NotificationHandlers;

/// <summary>
/// Конечная цель вызвать обработчик
/// </summary>
internal sealed class UserRegisteredIntegrationEventHandler(ILogger<UserRegisteredIntegrationEventHandler> logger)
    : INotificationHandler<UserRegisteredIntegrationEvent>
{
    public Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User registration handled {user}", notification.Id);
        // Asynchronously handle the event.
        return Task.CompletedTask;
    }
}