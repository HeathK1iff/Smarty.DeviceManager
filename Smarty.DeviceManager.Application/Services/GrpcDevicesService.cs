using Grpc.Core;
using Smarty.DeviceManager.Domain.Entities;
using Smarty.DeviceManager.Domain.Interfaces;


namespace Smarty.DeviceManager.Application.Services;

public class GrpcDevicesService: Devices.DevicesBase
{
    readonly IDeviceManager _deviceService;
    public GrpcDevicesService(IDeviceManager deviceService)
    {
        _deviceService = deviceService;
    }

    public override async Task<AddDeviceResponse> AddDevice(AddDeviceRequest request, ServerCallContext context)
    {
        var device = await _deviceService.AddAsync(new Device(){
            
        });

        return new AddDeviceResponse(){
            DeviceId = device.Id.ToString()
        };
    }

    public override async Task<GetDevicesReply> GetDevicesByProtocol(GetDevicesByProtocolRequest request, ServerCallContext context)
    {
        var devices = await _deviceService.GetDevicesByProtocolAsync(request.Protocol);
        
        var reply = new GetDevicesReply();
        
        foreach (var device in devices)
        {
            reply.Devices.Add(
                new DeviceObject(){
                    Id = device.Id.ToString(),
                    ParentId = device.ParentId.ToString(),
                    Vendor = device.Vendor,
                    Model = device.Model,
                    ConnectionString = device.ConnectionString
            });
        }
        
        return reply;
    }
}
