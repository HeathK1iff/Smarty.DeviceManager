using Smarty.DeviceManager.Application.Models;
using Smarty.DeviceManager.Domain.Entities;

namespace Smarty.DeviceManager.Application.Extensions;

public static class DeviceModelExtensions
{
    public static Device ToEntity(this DeviceModel model)
    {
        return new Device()
        {
            Id = model.Id,
            ConnectionString = model.ConnectionString,
            Location = model.Location,
            Model = model.Model,
            ParentId = model.ParentId,
            Vendor = model.Vendor
        };
    }

    public static Device ToEntity(this DeviceAddModel model)
    {
        return new Device()
        {
            ConnectionString = model.ConnectionString,
            Location = model.Location,
            Model = model.Model,
            ParentId = model.ParentId,
            Vendor = model.Vendor
        };
    }

}