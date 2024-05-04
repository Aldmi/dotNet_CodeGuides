using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace RxConsumer;

public class SystemTextJsonSerDe<T> : ISerializer<T>, IDeserializer<T>
{
    private JsonSerializerOptions DefaultOptions { get; }

    public SystemTextJsonSerDe(JsonSerializerOptions defaultOptions)
        => DefaultOptions = defaultOptions;

    public byte[] Serialize(T data, SerializationContext context)
        => JsonSerializer.SerializeToUtf8Bytes(data, DefaultOptions);

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<T>(data)!;
    }
}