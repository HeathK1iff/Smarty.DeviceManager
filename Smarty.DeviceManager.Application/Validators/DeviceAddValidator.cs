using FluentValidation;
using Smarty.DeviceManager.Application.Models;
using Smarty.DeviceManager.Domain.Entities;

namespace Smarty.DeviceManager.Application.Validators;

public class DeviceAddValidator : AbstractValidator<DeviceAddModel>
{
    public DeviceAddValidator()
    {
        RuleFor(a => a.Model)
            .NotNull()
            .WithName("model");

        RuleFor(a => a.Vendor)
            .NotNull()
            .WithName("vendor");

        RuleFor(a => a.ConnectionString)
            .NotNull()
            .Matches(Device.ConnectionStringFormat)
            .WithName("connection string");
    }
}
