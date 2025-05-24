using Smarty.DeviceManager.Application.Models;
using Smarty.DeviceManager.Domain.Entities;

namespace Smarty.DeviceManager.Application.Extensions;

public static class DeviceExtensions
{
    public static DeviceModel ToModel(this Device device)
    {
        return new DeviceModel()
        {
            Location = device.Location,
            Model = device.Model,
            ConnectionString = device.ConnectionString,
            Id = device.Id,
            ParentId = device.ParentId,
            Vendor = device.Vendor
        };
    }
}
