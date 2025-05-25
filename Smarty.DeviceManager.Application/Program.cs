using FluentMigrator.Runner;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Smarty.DeviceManager.Application.Extensions;
using Smarty.DeviceManager.Application.Interfaces;
using Smarty.DeviceManager.Application.Services;
using Smarty.DeviceManager.Domain.Interfaces;
using Smarty.DeviceManager.Domain.Services;
using Smarty.DeviceManager.Infrastructure;
using Smarty.DeviceManager.Infrastructure.Db.Migrations;
using Smarty.DeviceManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddEventBusService(builder.Configuration);
builder.Services.AddScoped<IDeviceManager, DeviceManager>();
builder.Services.AddScoped<IApiDeviceService, ApiDeviceService>();
builder.Services.AddScoped<IDevicesRepository>(serviceProvider =>
    {
        var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
        var dbContext = serviceProvider.GetRequiredService<IDbContext>();
        return new CachedDevicesRepository(new DevicesRepository(dbContext), memoryCache);
    });
builder.Services.AddScoped<IDbContext, DbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("db"))
                    .ScanIn(typeof(M202505111541_InitTables).Assembly).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

//if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.MapGrpcService<GrpcDevicesService>();
app.MapControllers();

await app.RunAsync();
