using System.Transactions;
using Microsoft.Extensions.Caching.Memory;
using Smarty.DeviceManager.Domain.Entities;
using Smarty.DeviceManager.Domain.Interfaces;

namespace Smarty.DeviceManager.Infrastructure.Repositories;

public sealed class CachedDevicesRepository : IDevicesRepository
{
    static TimeSpan s_expiryTimeOut = TimeSpan.FromSeconds(15);
    static Guid s_listKey = Guid.NewGuid();
    static object _lock = new();
    readonly IDevicesRepository _devicesRepository;
    readonly IMemoryCache _memoryCache;

    public CachedDevicesRepository(IDevicesRepository devicesRepository,
        IMemoryCache memoryCache)
    {
        _devicesRepository = devicesRepository ?? throw new ArgumentNullException(nameof(devicesRepository));
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
    }

    public async Task DeleteAsync(Guid id)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Suppress))
        {
            await _devicesRepository.DeleteAsync(id);

            lock (_lock)
            {
                _memoryCache.Remove(id);

                if (_memoryCache.TryGetValue(s_listKey, out HashSet<Guid>? list))
                {
                    list?.Remove(id);
                }
            }

            scope.Complete();
        }
    }

    public async Task<IEnumerable<Device>> GetAllOrEmptyAsync()
    {
        return await _memoryCache.GetOrCreateAsync(s_listKey, async factory =>
        {
            return await _devicesRepository.GetAllOrEmptyAsync() ;
        }) ?? Enumerable.Empty<Device>();
    }

    public async Task<IEnumerable<Device>> GetDevicesByProtocolAsync(string protocol)
    {
       return await _memoryCache.GetOrCreateAsync(s_listKey, async factory =>
        {
            factory.SetSlidingExpiration(TimeSpan.FromSeconds(15));
            return await _devicesRepository.GetDevicesByProtocolAsync(protocol);
        }) ?? Enumerable.Empty<Device>();
    }

    public async Task InsertAsync(Device entity)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _devicesRepository.InsertAsync(entity);

            lock (_lock)
            {
                AddToCache(entity);
            }

            scope.Complete();
        }
    }

    public async Task UpdateAsync(Device entity)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Suppress))
        {
            await _devicesRepository.UpdateAsync(entity);

            lock (_lock)
            {
                _memoryCache.Remove(entity.Id);
                AddToCache(entity);
            }

            scope.Complete();
        }
    }

    private Task AddToCache(Device entity)
    {
        var cacheEntry = _memoryCache.CreateEntry(entity.Id);
        cacheEntry.SetValue(entity);
        cacheEntry.SlidingExpiration = s_expiryTimeOut;

        if (_memoryCache.TryGetValue(s_listKey, out HashSet<Guid>? list))
        {
            list?.Add(entity.Id);
        }

        return Task.CompletedTask;
    }
}