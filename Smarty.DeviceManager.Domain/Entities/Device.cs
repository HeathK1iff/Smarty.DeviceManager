using System.Text.RegularExpressions;

namespace Smarty.DeviceManager.Domain.Entities;

public sealed class Device : EntityBase
{
    public static string ConnectionStringFormat = @"(\w{1,5})\:\/\/(.+)"; 
    string _connectionString = string.Empty;

    public Guid ParentId { get; init; } 
    public string Vendor { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string ConnectionString 
    { 
        get => _connectionString; 
        init
        {
            if (!Regex.IsMatch(value, ConnectionStringFormat))
            {
                throw new FormatException();
            }

            _connectionString = value.ToLowerInvariant();
        } 
    }

    public string Protocol
    {
        get 
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return string.Empty;
            }
            
            var match = Regex.Match(_connectionString, ConnectionStringFormat);

            if (match.Success)
            {
                return match.Groups.Count > 0 ? match.Groups[1].Value : string.Empty; 
            }

            return string.Empty;
        }
    }
}

