using MediatR;

namespace MediatrFeatures.Notifications;

/// <summary>
/// Обработчик 1
/// </summary>
internal class OneWetherWarningNotificationHandler : INotificationHandler<WetherWarningNotification>
{
    public async Task Handle(WetherWarningNotification notification, CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);
        Console.WriteLine("Hello from OneWetherWarningNotificationHandler");
    }
}