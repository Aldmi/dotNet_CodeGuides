using MediatR;

namespace MediatrFeatures.Notifications;

/// <summary>
/// Обработчик 2
/// </summary>
internal class TwoWetherWarningNotificationHandler : INotificationHandler<WetherWarningNotification>
{
    public async Task Handle(WetherWarningNotification notification, CancellationToken cancellationToken)
    {
        await Task.Delay(3000, cancellationToken);
        Console.WriteLine("Hello from TwoWetherWarningNotificationHandler");
    }
}