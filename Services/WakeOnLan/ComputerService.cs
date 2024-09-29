using System.Text.Json;
using static System.IO.File;

namespace Rstolsmark.WakeOnLanServer.Services.WakeOnLan;

public class ComputerService
{

    public ComputerService()
    {
        computersPath = $"{AppDomain.CurrentDomain.GetData("DataDirectory")}{Path.DirectorySeparatorChar}computers.json";
        _computers = new Lazy<Computer[]>(GetComputersFromFile);
        _computerDictionary = new Lazy<Dictionary<string, Computer>>(CreateComputerDictionary);
    }

    private string computersPath;

    private Lazy<Computer[]> _computers;
    private Lazy<Dictionary<string, Computer>> _computerDictionary;

    public Computer[] GetAllComputers()
    {
        return _computers.Value;
    }

    private Dictionary<string, Computer> GetComputerDictionary()
    {
        return _computerDictionary.Value;
    }

    private Dictionary<string, Computer> CreateComputerDictionary()
    {
        return GetAllComputers().ToDictionary(c => c.Name);
    }

    public Computer GetComputerByName(string name)
    {
        if(!DoesComputerExist(name)){
            return null;
        }
        var computers = GetComputerDictionary();
        return computers[name];
    }

    public bool DoesComputerExist(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }
        return GetComputerDictionary().ContainsKey(name);
    }

    private void AddOrUpdateComputer(string name, Computer computer)
    {
        computer.StandardizeMacAddress();
        var computers = GetComputerDictionary();
        computers[name] = computer;
        Save(computers);
    }

    public void AddComputer(Computer computer)
    {
        AddOrUpdateComputer(computer.Name, computer);
    }

    public void EditComputer(string name, Computer computer)
    {
        AddOrUpdateComputer(name, computer);
    }

    public void Delete(string name)
    {
        var computers = GetComputerDictionary();
        if (computers.Remove(name))
        {
            Save(computers);
        }
    }

    private Computer[] GetComputersFromFile()
    {
        if (!Exists(computersPath))
        {
            return new Computer[] { };
        }
        return JsonSerializer.Deserialize<Computer[]>(ReadAllText(computersPath));
    }

    public Task<ComputerWithAwakeDto[]> GetAllComputersWithAwakeStatus()
    {
        var computers = GetAllComputers();
        return Task.WhenAll(computers.Select(ToComputerWithAwakeDto).ToArray());
    }

    public async Task<ComputerWithAwakeDto> GetComputerWithAwakeStatusByName(string name){
        var computer = GetComputerByName(name);
        if(computer == null){
            return null;
        }
        return await ToComputerWithAwakeDto(computer);
    }

    private void Save(Dictionary<string, Computer> computers)
    {
        var newComputerList = computers.Values.ToList();
        new FileInfo(computersPath).Directory.Create();
        WriteAllText(computersPath, JsonSerializer.Serialize(newComputerList, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static async Task<ComputerWithAwakeDto> ToComputerWithAwakeDto(Computer c)
    {
        var computerIsAwake = await c.Ping();
        return new ComputerWithAwakeDto(c, computerIsAwake);
    }
}
