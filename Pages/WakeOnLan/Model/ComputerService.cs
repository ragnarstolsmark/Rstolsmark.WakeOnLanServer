using System.Text.Json;
using static System.IO.File;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

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

    public Computer[] GetAllComputers(){
        return _computers.Value;
    }

    private Dictionary<string, Computer> GetComputerDictionary(){
        return _computerDictionary.Value;
    }

    private Dictionary<string, Computer> CreateComputerDictionary()
    {
        return GetAllComputers().ToDictionary(c => c.Name);
    }

    public Computer GetComputerByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var computers = GetComputerDictionary();
        if (!computers.ContainsKey(name))
        {
            return null;
        }
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
        if(computers.Remove(name)){
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

    private void Save(Dictionary<string, Computer> computers)
    {
        var newComputerList = computers.Values.ToList();
        (new FileInfo(computersPath)).Directory.Create();
        WriteAllText(computersPath, JsonSerializer.Serialize(newComputerList, new JsonSerializerOptions { WriteIndented = true }));
    }
}
