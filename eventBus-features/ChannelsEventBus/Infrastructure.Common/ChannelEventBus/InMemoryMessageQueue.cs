using System.Threading.Channels;
using Application.Abstract;

namespace Infrastructure.Common.ChannelEventBus;

internal sealed class InMemoryMessageQueue
{
    private readonly Channel<IIntegrationEvent> _channel =
        Channel.CreateUnbounded<IIntegrationEvent>();

    public ChannelReader<IIntegrationEvent> Reader => _channel.Reader;

    public ChannelWriter<IIntegrationEvent> Writer => _channel.Writer;
}