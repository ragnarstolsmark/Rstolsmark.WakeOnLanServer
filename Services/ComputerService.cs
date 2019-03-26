using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using WakeOnLanServer.Model;

namespace WakeOnLanServer.Services {
    public static class ComputerService {
        public static Computer[] GetAllComputers(){
            if(!System.IO.File.Exists(@"computers.json")){
                return new Computer[]{};
            }
            return JsonConvert.DeserializeObject<Computer[]>(System.IO.File.ReadAllText(@"computers.json"));
        }
        public static Dictionary<string, Computer> GetComputerDictionary(){
            return GetAllComputers().ToDictionary(c => c.Name);
        }
    }
}