using Application.Abstract;

namespace Application.Features.RegisterUser;

internal record UserRegisteredIntegrationEvent(Guid Id) : IIntegrationEvent;