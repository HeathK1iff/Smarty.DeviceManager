using LinqToDB.Mapping;

namespace Smarty.DeviceManager.Infrastructure.Db.Entities;

[Table("Devices")]
public sealed class DeviceDb
{
    [PrimaryKey]
    public Guid Id { get; init; }

    [Column("ParentId")]
    public Guid? ParentId { get; init; }

    [Column("Vendor")]
    public string Vendor { get; set; } = string.Empty;

    [Column("Model")]
    public string Model { get; set; } = string.Empty;

    [Column("Location")]
    public string Location { get; set; }  = string.Empty;

    [Column("ConnectionString")]
    public string ConnectionString { get; set; } = string.Empty;
}