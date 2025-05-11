using Smarty.DeviceManager.Domain.Entities;

namespace Smarty.DeviceManager.Domain.Interfaces;

public interface IDeviceManager
{
    Task<Device> AddAsync(Device device);
    Task<Device[]> GetDevicesByProtocolAsync(string protocol);
}
