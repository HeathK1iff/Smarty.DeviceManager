using FluentMigrator.Runner;
using Smarty.DeviceManager.Application.Extensions;
using Smarty.DeviceManager.Application.Services;
using Smarty.DeviceManager.Domain.Interfaces;
using Smarty.DeviceManager.Domain.Services;
using Smarty.DeviceManager.Infrastructure;
using Smarty.DeviceManager.Infrastructure.Db.Migrations;
using Smarty.DeviceManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddMemoryCache();
builder.Services.AddEventBusService(builder.Configuration);
builder.Services.AddTransient<IDeviceManager, DeviceManager>();
builder.Services.AddTransient<IDevicesRepository, DevicesRepository>();
builder.Services.AddTransient<IDbContext, DbContext>();
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



app.MapGrpcService<GrpcDevicesService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
