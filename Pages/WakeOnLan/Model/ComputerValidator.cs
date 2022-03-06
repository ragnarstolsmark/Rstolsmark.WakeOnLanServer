using FluentValidation;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

public class ComputerValidator : AbstractValidator<Computer>
{
    public ComputerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();

        RuleFor(p => p.IP)
            .NotEmpty();

        RuleFor(p => p.MAC)
            .NotEmpty();

        RuleFor(p => p.SubnetMask)
            .NotEmpty();
    }
}