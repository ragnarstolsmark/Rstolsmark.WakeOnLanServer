using FluentValidation;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;
#nullable enable

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

public class ComputerValidator : AbstractValidator<Computer>
{
    public ComputerValidator(ComputerService computerService, string? previousName = null)
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .Must(name => 
            {
                if(previousName == name){
                    return true;
                }
                return !computerService.DoesComputerExist(name);
            })
                .WithMessage("'{PropertyValue}' already exists.");

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