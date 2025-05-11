using System.Text;
using System.Text.Json;
using Smarty.Shared.EventBus.Abstractions.Events;
using Smarty.Shared.EventBus.Interfaces;

namespace Smarty.DeviceManager.Infrastructure.EventBus;

public sealed class EventSerializator : IEventSerializator
{
    public async Task<EventBase?> DeserializeAsync(byte[] content, Type type, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream(content);

        return await JsonSerializer.DeserializeAsync(stream, type, cancellationToken: cancellationToken) as EventBase;
    }

    public byte[] Serialize<T>(T content) where T: EventBase
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var jsonText = JsonSerializer.Serialize(content, options);
        return Encoding.UTF8.GetBytes(jsonText);
    }
}