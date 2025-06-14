using Smarty.DeviceManager.Domain.Events;

public interface IUserAddEventPublisher
{
    Task PublishAsync(UserAddEvent @event, CancellationToken token);
}
