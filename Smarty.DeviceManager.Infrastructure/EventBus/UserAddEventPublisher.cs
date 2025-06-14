using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Smarty.DeviceManager.Domain.Events;

public sealed class UserAddEventPublisher: IUserAddEventPublisher
{
    readonly EventBusOptions _options;
    readonly IChannel _channel;

    public UserAddEventPublisher(IChannel channel, EventBusOptions options)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task PublishAsync(UserAddEvent @event, CancellationToken token)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        await _channel.BasicPublishAsync(_options.ExchangeName, _options.UserAddKey, bytes, token);
    }
}