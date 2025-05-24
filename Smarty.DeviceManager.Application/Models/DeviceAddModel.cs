
using System.Runtime.Serialization;

namespace Smarty.DeviceManager.Application.Models;

[DataContract]
public sealed class DeviceAddModel
{
    [DataMember]
    public Guid ParentId { get; init; }

    [DataMember]
    public string Vendor { get; init; } = string.Empty;

    [DataMember]
    public string Model { get; init; } = string.Empty;

    [DataMember]
    public string Location { get; init; } = string.Empty;

    [DataMember]
    public string ConnectionString { get; init; } = string.Empty;
}
