public sealed class EventBusOptions
{
    public string HostName { get; init; } = string.Empty;
    public int Port { get; init; }
    public string ExchangeName { get; init; } = string.Empty;
    public string UserAddQueueName { get; init; } = string.Empty;
    public string UserAddKey { get; init; } = string.Empty;
}