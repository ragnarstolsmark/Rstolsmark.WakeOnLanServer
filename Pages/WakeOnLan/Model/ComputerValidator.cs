using FluentValidation;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

public class ComputerValidator : AbstractValidator<Computer>
{
    public ComputerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();

        RuleFor(p => p.IP)
            .NotEmpty()
            .BeAValidIpAddress();

        RuleFor(p => p.MAC)
            .NotEmpty()
            .BeAValidMacAddress();

        RuleFor(p => p.SubnetMask)
            .NotEmpty()
            .BeAValidIpAddress();//todo: only allow valid sub net masks
    }
}