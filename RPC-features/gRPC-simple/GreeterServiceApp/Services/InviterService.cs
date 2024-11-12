using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GreeterServiceApp.Services;
using InviterServiceApp;

public class InviterService : Inviter.InviterBase
{
    public override Task<Response> Invite(Request request, ServerCallContext context)
    {
        // начало мероприятия - условно следующий день 
        var eventDateTime = DateTime.UtcNow.AddDays(1);
        var eventDutation = TimeSpan.FromHours(2);

        return Task.FromResult(new Response
        {
            Invitation = $"{request.Name}, приглашаем вас посетить мероприятие",
            Start = Timestamp.FromDateTime(eventDateTime),
            Duration = Duration.FromTimeSpan(eventDutation)
        });
    }
}