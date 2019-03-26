using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WakeOnLanServer.Model;
using static System.IO.File;
namespace WakeOnLanServer.Services {
    public static class ComputerService {
        public static Computer[] GetAllComputers(){
            if(!Exists("computers.json")){
                return new Computer[]{};
            }
            return JsonConvert.DeserializeObject<Computer[]>(ReadAllText("computers.json"));
        }
        public static Dictionary<string, Computer> GetComputerDictionary(){
            return GetAllComputers().ToDictionary(c => c.Name);
        }

        public static void AddOrUpdateComputer(Computer computer){
            var computers = GetAllComputers();
            var computerExists = false;
            for(var i=0;i<computers.Length;i++){
                var comp = computers[i];
                if(computer.Name == comp.Name){
                    computers[i] = computer;
                    computerExists = true;
                    break;
                }
            }
            var newComputerList = computers.ToList();
            if(!computerExists){
                newComputerList.Add(computer);
            }
            WriteAllText("computers.json", JsonConvert.SerializeObject(newComputerList, Formatting.Indented));
        }
    }
}