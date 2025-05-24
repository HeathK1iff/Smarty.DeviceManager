using Smarty.DeviceManager.Domain.Entities;

namespace Smarty.DeviceManager.Domain.Interfaces;
public interface IDevicesRepository
{
    Task<IEnumerable<Device>> GetDevicesByProtocolAsync(string protocol);  
    Task<IEnumerable<Device>> GetAllOrEmptyAsync();
    Task InsertAsync(Device entity);
    Task UpdateAsync(Device entity);
    Task DeleteAsync(Guid id);
}
