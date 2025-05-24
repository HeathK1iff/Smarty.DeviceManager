using Smarty.DeviceManager.Domain.Entities;

namespace Smarty.DeviceManager.Domain.Interfaces;

public interface IDeviceManager
{
    Task<Device> AddAsync(Device device);

    Task<IEnumerable<Device>> GetAllOrEmpty();

    Task<IEnumerable<Device>> GetDevicesByProtocolAsync(string protocol);
}
