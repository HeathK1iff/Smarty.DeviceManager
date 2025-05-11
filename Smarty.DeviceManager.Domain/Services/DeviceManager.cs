using Smarty.DeviceManager.Domain.Entities;
using Smarty.DeviceManager.Domain.Events;
using Smarty.DeviceManager.Domain.Interfaces;
using Smarty.Shared.EventBus.Interfaces;

namespace Smarty.DeviceManager.Domain.Services;

public sealed class DeviceManager : IDeviceManager
{
    readonly IDevicesRepository _devicesRepository;
    readonly IEventBusChannelFactory _eventBusChannelFactory;

    public DeviceManager(IDevicesRepository devicesRepository, IEventBusChannelFactory eventBusChannelFactory)
    {
        _devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
        _eventBusChannelFactory = eventBusChannelFactory ?? throw new ArgumentNullException(nameof(eventBusChannelFactory));
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

        var publisher = _eventBusChannelFactory.CreatePublisher();

        await publisher.PublishAsync(new UserAddEvent()
        {
            DeviceId = device.Id,
            ParentId = device.ParentId,
            Model = device.Model,
            Vendor = device.Vendor,
            ConnectionString = device.ConnectionString,
            Version = 1
        }, CancellationToken.None);

        return newDevice;
    }

    public async Task<Device[]> GetDevicesByProtocolAsync(string protocol)
    {
        var devices = await _devicesRepository
            .GetAllOrEmptyAsync();

        return devices
            .Where(a => a.Protocol == protocol)
            .ToArray();
    }
}
