using FluentMigrator;

namespace Smarty.DeviceManager.Infrastructure.Db.Migrations;

[Migration(202505111541)]
public class M202505111541_InitTables : Migration
{
    public override void Down()
    {
        Delete.Table("Devices");
    }

    public override void Up()
    {
        Create.Table("Devices")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ParentId").AsGuid().Nullable().ForeignKey("Devices", "Id")
            .WithColumn("Vendor").AsString().NotNullable()
            .WithColumn("Model").AsString().NotNullable()
            .WithColumn("Location").AsString().Nullable()
            .WithColumn("ConnectionString").AsString().NotNullable();
    }
}
