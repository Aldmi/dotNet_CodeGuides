using Domain.Books.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Core.BookFeatures.Command.CreateBook;

public class SendEmailCreateBookDomainEventHandler(ILogger<SendEmailCreateBookDomainEventHandler> logger)
    : INotificationHandler<CreateBookDomainEvent>
{
    private readonly ILogger<SendEmailCreateBookDomainEventHandler> _logger = logger;

    public Task Handle(CreateBookDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Send Email Domain Event");
        return Task.CompletedTask;
    }
}