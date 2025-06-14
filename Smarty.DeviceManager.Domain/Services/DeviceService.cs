using Smarty.DeviceManager.Domain.Entities;
using Smarty.DeviceManager.Domain.Events;
using Smarty.DeviceManager.Domain.Interfaces;

namespace Smarty.DeviceManager.Domain.Services;

public sealed class DeviceService : IDeviceService
{
    readonly IDevicesRepository _devicesRepository;
    readonly IEventBusPublisherFactory _eventBusPublisherFactory;

    public DeviceService(IDevicesRepository devicesRepository,
        IEventBusPublisherFactory eventBusPublisherFactory)
    {
        _devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
        _eventBusPublisherFactory = eventBusPublisherFactory ?? throw new ArgumentNullException(nameof(eventBusPublisherFactory));
    }

    public async Task<Device> AddAsync(Device device)
    {   
        var newDevice = new Device()
        {
            Id = Guid.NewGuid(),
            ParentId = device.ParentId,
            Vendor = device.Vendor,
            Model = device.Model,
            Location = device.Location,
            ConnectionString = device.ConnectionString
        };

        await _devicesRepository.InsertAsync(newDevice);

        var publisher = await _eventBusPublisherFactory.CreateUserAddEventPublisherAsync();
        
        await publisher.PublishAsync(new UserAddEvent()
        {
            DeviceId = device.Id,
            ParentId = device.ParentId,
            Model = device.Model,
            Vendor = device.Vendor,
            ConnectionString = device.ConnectionString
        }, CancellationToken.None);

        return newDevice;
    }

    public async Task<IEnumerable<Device>> GetAllOrEmpty()
    {
        return await _devicesRepository
            .GetAllOrEmptyAsync();
    }

    public async Task<IEnumerable<Device>> GetDevicesByProtocolAsync(string protocol)
    {
        return await _devicesRepository
            .GetDevicesByProtocolAsync(protocol);
    }
}
