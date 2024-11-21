using Domain.Books.DomainEvents;
using MediatR;

namespace Application.Core.BookFeatures.DomainEventHandler;

public class SendEmailCreateBookDomainEventHandler : INotificationHandler<CreateBookDomainEvent>
{
    public Task Handle(CreateBookDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
        //Логика отправки письма
    }
}