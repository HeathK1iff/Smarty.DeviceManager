using Microsoft.Extensions.Options;
using RabbitMQ.Client;

public sealed class EventBusPublisherFactory : IEventBusPublisherFactory, IDisposable
{
    readonly ConnectionFactory _connectionFactory;
    readonly EventBusOptions _options;
    IConnection? _connection = null;
    bool _disposedValue;

    public EventBusPublisherFactory(IOptions<EventBusOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        _connectionFactory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            Port = _options.Port
        };
    }

    public async Task ConnectAsync()
    {
        if (_connection is not null)
        {
            throw new InvalidOperationException();
        }

        _connection = await _connectionFactory.CreateConnectionAsync();

        using (var channel = await _connection.CreateChannelAsync())
        {
            await channel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Direct, durable: true);
            
            await channel.QueueDeclareAsync(_options.UserAddQueueName, durable: true);
            await channel.QueueBindAsync(_options.UserAddQueueName, _options.ExchangeName, _options.UserAddKey);
        }
    }

    public async Task<IUserAddEventPublisher> CreateUserAddEventPublisherAsync()
    {
        if (_connection is null)
        {
            throw new InvalidOperationException();
        }

        var channel = await _connection.CreateChannelAsync();

        return new UserAddEventPublisher(channel, _options);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
