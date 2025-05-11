using Smarty.Shared.EventBus.Abstractions.Events;

namespace Smarty.DeviceManager.Domain.Events;

public class UserAddEvent: EventBase
{
    public Guid DeviceId { get; init; } 
    public Guid ParentId { get; init; } 
    public string Vendor { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
} 
