using FluentValidation;
using Rstolsmark.WakeOnLanServer.ValidationHelpers;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public class PortForwardingFormValidator : AbstractValidator<PortForwardingForm>
{
    public PortForwardingFormValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(p => p.Protocol)
            .BeAValidProtocol();

        RuleFor(p => p.SourceIp)
            .BeAValidIpAddress()
                .When(p => !string.IsNullOrWhiteSpace(p.SourceIp));
        
        RuleFor(p => p.SourcePort)
            .BeAValidPort();

        RuleFor(p => p.DestinationIp)
            .NotEmpty()
            .BeAValidIpAddress();
        
        RuleFor(p => p.DestinationPort)
            .BeAValidPort();
    }
}