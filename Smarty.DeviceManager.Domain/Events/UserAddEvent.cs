namespace Smarty.DeviceManager.Domain.Events;

public sealed class UserAddEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid DeviceId { get; init; } 
    public Guid? ParentId { get; init; } 
    public string Vendor { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
} 
