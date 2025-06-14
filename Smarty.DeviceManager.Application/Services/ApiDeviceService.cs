using FluentValidation;
using Smarty.DeviceManager.Application.Extensions;
using Smarty.DeviceManager.Application.Interfaces;
using Smarty.DeviceManager.Application.Models;
using Smarty.DeviceManager.Application.Validators;
using Smarty.DeviceManager.Domain.Interfaces;

namespace Smarty.DeviceManager.Application.Services;

public sealed class ApiDeviceService : IApiDeviceService
{
    readonly IDeviceService _deviceManager;
    public ApiDeviceService(IDeviceService deviceManager)
    {
        _deviceManager = deviceManager;
    }

    public async Task<DeviceModel> AddAsync(DeviceAddModel model)
    {
        await new DeviceAddValidator().ValidateAndThrowAsync(model);

        var newModel = await _deviceManager.AddAsync(model.ToEntity());
        return newModel.ToModel();
    }

    public async Task<IEnumerable<DeviceModel>> GetAllAsync()
    {
        var items = await _deviceManager.GetAllOrEmpty();

        return items.Select(device => device.ToModel());
    }
}
