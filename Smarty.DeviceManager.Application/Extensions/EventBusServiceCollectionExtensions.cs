using Smarty.DeviceManager.Infrastructure.EventBus;
using Smarty.Shared.EventBus;
using Smarty.Shared.EventBus.Interfaces;

namespace Smarty.DeviceManager.Application.Extensions;

public static class EventBusServiceCollectionExtensions
{
    public static void AddEventBusService(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        serviceDescriptors.AddSingleton<IEventBusChannelFactory>(a=>{
            return new EventBusChannelFactory(configuration?.GetConnectionString("eventBus") ?? string.Empty,
                a.GetRequiredService<IEventQueueResolver>(),
                a.GetRequiredService<IServiceProvider>());
            });
        serviceDescriptors.AddSingleton<IEventQueueResolver, EventQueueResolver>();
        serviceDescriptors.AddSingleton<IEventSerializator, EventSerializator>();
    }
}
