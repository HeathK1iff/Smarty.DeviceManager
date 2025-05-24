using Microsoft.AspNetCore.Mvc;
using Smarty.DeviceManager.Application.Interfaces;
using Smarty.DeviceManager.Application.Models;

namespace Smarty.DeviceManager.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    readonly IApiDeviceService _apiDeviceService;
    public DevicesController(IApiDeviceService apiDeviceService)
    {
        _apiDeviceService = apiDeviceService ?? throw new ArgumentNullException(nameof(apiDeviceService));
    }

    [HttpPost]
    public async Task<ActionResult<DeviceModel>> Add(DeviceAddModel model)
    {
        try
        {
            return Ok(await _apiDeviceService.AddAsync(model));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpPut]
    public Task Update(DeviceModel model)
    {
        return Task.CompletedTask;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceModel>>> GetAll()
    {
        try
        {
            return Ok(await _apiDeviceService.GetAllAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
