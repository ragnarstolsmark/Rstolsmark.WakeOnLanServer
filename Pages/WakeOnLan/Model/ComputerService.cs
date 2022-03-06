using System.Text.Json;
using static System.IO.File;

namespace Rstolsmark.WakeOnLanServer.Pages.WakeOnLan.Model;

public static class ComputerService
{
    private static string computersPath => $"{AppDomain.CurrentDomain.GetData("DataDirectory")}{Path.DirectorySeparatorChar}computers.json";
    public static Computer[] GetAllComputers()
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

    public static void AddOrUpdateComputer(Computer computer)
    {
        computer.StandardizeMacAddress();
        var computers = GetAllComputers();
        var computerExists = false;
        for (var i = 0; i < computers.Length; i++)
        {
            var comp = computers[i];
            if (computer.Name == comp.Name)
            {
                computers[i] = computer;
                computerExists = true;
                break;
            }
        }
        var newComputerList = computers.ToList();
        if (!computerExists)
        {
            newComputerList.Add(computer);
        }
        (new FileInfo(computersPath)).Directory.Create();
        WriteAllText(computersPath, JsonSerializer.Serialize(newComputerList, new JsonSerializerOptions { WriteIndented = true }));
    }
}