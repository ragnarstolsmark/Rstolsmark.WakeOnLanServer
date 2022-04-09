using System.Reflection;

namespace Rstolsmark.WakeOnLanServer.Services;

public class ProgramVersion
{
    private readonly string _version;
    public ProgramVersion()
    {
        _version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            .InformationalVersion;
    }

    public string GetVersion() => _version;
}