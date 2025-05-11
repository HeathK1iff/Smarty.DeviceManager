using LinqToDB;
using Smarty.DeviceManager.Domain.Entities;
using Smarty.DeviceManager.Domain.Interfaces;
using Smarty.DeviceManager.Infrastructure.Db.Entities;

namespace Smarty.DeviceManager.Infrastructure.Repositories;

public sealed class DevicesRepository : IDevicesRepository
{   
    readonly IDbContext _dataContext;
    public DevicesRepository(IDbContext dataContext)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    }

    public async Task<Device[]> GetAllOrEmptyAsync()
    {   
        var items = await _dataContext.Devices.ToArrayAsync() ?? Array.Empty<DeviceDb>();
        
        return items
            .Select(a =>
                new Device
                {
                    ParentId = a.ParentId, 
                    Vendor = a.Vendor,
                    Model = a.Model,
                    ConnectionString = a.ConnectionString
                }).ToArray();
    }

    public async Task InsertAsync(Device entity)
    {
        var newItem = new DeviceDb()
        {
            Id = entity.Id,
            Vendor = entity.Vendor,
            Model = entity.Model,
            ParentId = entity.ParentId,
            ConnectionString = entity.ConnectionString
        };
        
        await _dataContext.Devices.InsertAsync(() => newItem);
    }

    public async Task UpdateAsync(Device entity)
    {
        var updateItem = await _dataContext
            .Devices
            .FirstOrDefaultAsync(a=>a.Id == entity.Id);

        if (updateItem is null)
        {
            throw new InvalidDataException();
        }
        
        await _dataContext.Devices
            .Where(a => a.Id == entity.Id)
            .Set(a => a.Model, entity.Model)
            .Set(a => a.Vendor, entity.Vendor)
            .Set(a => a.ConnectionString, entity.ConnectionString)
            .UpdateAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dataContext.Devices
            .Where(a=>a.Id == id)
            .DeleteAsync();
    }
}