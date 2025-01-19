using Application.Abstract;
using Domain.Entities;
using MediatR;

namespace Application.Features.RegisterUser.Command;

public record RegisterUserCommand(Guid UserId, string Name) : IRequest<User>;


public class RegisterUserCommandHandler(IEventBus eventBus) : IRequestHandler<RegisterUserCommand, User>
{
    public async Task<User> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        // First, register the user.
        User user = new User() {Id = command.UserId, Name = command.Name};

        //userRepository.Insert(user);

        // Служба IEventBus запишет сообщение в Channelи немедленно вернется. Это позволяет публиковать сообщения неблокирующим способом, что может повысить производительность.
        //Если сразу публиковать INotification на шину то обработчики события выполняются последовательно.
        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(user.Id),
            cancellationToken);
        return user;
    }
}