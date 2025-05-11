using LinqToDB;
using Microsoft.Extensions.Configuration;
using Smarty.DeviceManager.Infrastructure.Db.Entities;

namespace Smarty.DeviceManager.Infrastructure;

public class DbContext: IDbContext
{
    readonly IConfiguration _configuration;
    readonly LinqToDB.DataContext _context;
    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        string? connectionString = _configuration.GetConnectionString("db");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidDataException();
        }

        var options = new DataOptions()
            .UsePostgreSQL(connectionString);

        _context = new LinqToDB.DataContext(options);
    }

    public ITable<DeviceDb> Devices => _context.GetTable<DeviceDb>();
}
