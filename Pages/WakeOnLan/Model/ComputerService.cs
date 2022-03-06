using System.Text.Json;
using static System.IO.File;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

public static class ComputerService
{
    private static string computersPath => $"{AppDomain.CurrentDomain.GetData("DataDirectory")}{Path.DirectorySeparatorChar}computers.json";
    private static Computer[] GetAllComputers()
    {
        if (!Exists(computersPath))
        {
            return new Computer[] { };
        }
        return JsonSerializer.Deserialize<Computer[]>(ReadAllText(computersPath));
    }
    public static Dictionary<string, Computer> GetComputerDictionary()
    {
        return GetAllComputers().ToDictionary(c => c.Name);
    }

    public static Computer GetComputerByName(string name)
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

    public static bool DoesComputerExist(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }
        return GetComputerDictionary().ContainsKey(name);
    }

    private static void AddOrUpdateComputer(string name, Computer computer)
    {
        computer.StandardizeMacAddress();
        var computers = GetComputerDictionary();
        computers[name] = computer;
        Save(computers);
    }
    
    public static void AddComputer(Computer computer)
    {
        AddOrUpdateComputer(computer.Name, computer);
    }

    public static void EditComputer(string name, Computer computer)
    {
        AddOrUpdateComputer(name, computer);
    }

    private static void Save(Dictionary<string, Computer> computers)
    {
        var newComputerList = computers.Values.ToList();
        (new FileInfo(computersPath)).Directory.Create();
        WriteAllText(computersPath, JsonSerializer.Serialize(newComputerList, new JsonSerializerOptions { WriteIndented = true }));
    }

    public static void Delete(string name)
    {
        var computers = GetComputerDictionary();
        if(computers.Remove(name)){
            Save(computers);
        }
    }
}