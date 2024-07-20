using System.Net;
using System.Net.NetworkInformation;
using FluentValidation;
using Rstolsmark.WakeOnLanServer.Services.PortForwarding;

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
    
    public static IRuleBuilderOptions<T, string> BeAValidMacAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(BeAValidMacAddress)
            .WithMessage("'{PropertyValue}' is not a valid MAC.");
    }
    
    private static bool BeAValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }
    
    private static bool BeAValidMacAddress(string macAddress)
    {
        return PhysicalAddress.TryParse(macAddress, out _);
    }
}