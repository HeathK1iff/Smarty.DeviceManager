using LinqToDB;
using Smarty.DeviceManager.Infrastructure.Db.Entities;

namespace Smarty.DeviceManager.Infrastructure;

public interface IDbContext
{
    ITable<DeviceDb> Devices { get; }

    Task<int> InsertAsync<T>(T item) where T: class;

}
