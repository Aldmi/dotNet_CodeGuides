using MediatR;

namespace MediatrFeatures.Notifications;

public class WetherWarningNotification : INotification
{
    public string WarningMessage { get; }

    public WetherWarningNotification(string warningMessage)
    {
        WarningMessage = warningMessage;
    }
}