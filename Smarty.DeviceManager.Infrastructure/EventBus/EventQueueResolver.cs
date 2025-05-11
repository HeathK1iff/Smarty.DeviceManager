using Smarty.DeviceManager.Domain.Events;
using Smarty.Shared.EventBus;
using Smarty.Shared.EventBus.Interfaces;

namespace Smarty.DeviceManager.Infrastructure.EventBus;

public class EventQueueResolver : IEventQueueResolver
{
    readonly IEventSerializator _eventSerializator;
    public EventQueueResolver(IEventSerializator eventSerializator)
    {
        _eventSerializator = eventSerializator ?? throw new ArgumentNullException(nameof(eventSerializator));
    }
    public bool TryGetQueue(Type eventType, out EventQueue? eventQueue)
    {
        eventQueue = null;

        if (eventType == typeof(UserAddEvent))
        {
            eventQueue = new EventQueue()
            {
                 QueueName = "userAdd",
                 Options = new QueueOptions()
                 {
                    Durable = false,
                    Exclusive = false
                 },
                 Serializator = _eventSerializator
            };

            return true;
        }

        return false;
    }
}