using System.Net;
using FluentValidation;

namespace Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

public class PortForwardingFormValidator : AbstractValidator<PortForwardingForm>
{
    private const string IpInvalidMessage = "'{PropertyValue}' is not a valid IP.";
    private const int MinPort = 1;
    private const int MaxPort = 65535;
    public PortForwardingFormValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
        
        RuleFor(p => p.Protocol)
            .Must(protocol => Enum.IsDefined(typeof(Protocol), protocol));

        RuleFor(p => p.SourceIp)
            .Must(BeAValidIpAddress)
                .WithMessage(IpInvalidMessage)
                .When(p => !string.IsNullOrWhiteSpace(p.SourceIp));
        
        RuleFor(p => p.SourcePort)
            .InclusiveBetween(MinPort, MaxPort);
        
        RuleFor(p => p.DestinationIp)
            .NotEmpty()
            .Must(BeAValidIpAddress)
                .WithMessage(IpInvalidMessage);
        
        RuleFor(p => p.DestinationPort)
            .InclusiveBetween(MinPort, MaxPort);
    }

    private bool BeAValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }
}