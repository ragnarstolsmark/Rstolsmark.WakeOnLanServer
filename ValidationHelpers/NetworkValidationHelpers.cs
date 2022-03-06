using System.Net;
using FluentValidation;
using Rstolsmark.WakeOnLanServer.Pages.PortForwarding.Model;

namespace Rstolsmark.WakeOnLanServer.ValidationHelpers;

public static class NetworkValidationHelpers
{
    public static IRuleBuilderOptions<T, Protocol> BeAValidProtocol<T>(this IRuleBuilder<T, Protocol> ruleBuilder)
    {
        return ruleBuilder.Must(protocol => Enum.IsDefined(typeof(Protocol), protocol));
    }
    public static IRuleBuilderOptions<T, int> BeAValidPort<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder.InclusiveBetween(1, 65535);
    }
    
    public static IRuleBuilderOptions<T, string> BeAValidIpAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(BeAValidIpAddress)
            .WithMessage("'{PropertyValue}' is not a valid IP.");
    }
    
    private static bool BeAValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }
}