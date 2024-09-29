namespace Rstolsmark.WakeOnLanServer.Services.WakeOnLan;
public class ComputerWithAwakeDto : Computer
{
    public ComputerWithAwakeDto(Computer computer, bool awake) : base(computer)
    {
        Awake = awake;
    }
    public bool Awake {get; init;}
}