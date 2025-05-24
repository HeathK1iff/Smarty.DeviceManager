using Smarty.DeviceManager.Application.Models;

namespace Smarty.DeviceManager.Application.Interfaces;

public interface IApiDeviceService
{
    Task<DeviceModel> AddAsync(DeviceAddModel model);

    Task<IEnumerable<DeviceModel>> GetAllAsync();
}
