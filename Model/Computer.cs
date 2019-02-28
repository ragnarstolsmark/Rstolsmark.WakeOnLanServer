namespace WakeOnLanServer.Model
{
    public class Computer
    {
        public string Name { get; set; }
        public string IP { get; set; }
        public string MAC { get; set; }
        public bool Woken {get;set;}
    }
}