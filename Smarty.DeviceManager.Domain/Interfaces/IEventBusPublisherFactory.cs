public interface IEventBusPublisherFactory
{
    Task<IUserAddEventPublisher> CreateUserAddEventPublisherAsync();
}
