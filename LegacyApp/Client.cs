namespace LegacyApp
{
    public class Client
    {
        public string Name { get; set; }
        public int ClientId { get; set; }

        public string Email { get; internal set; }
        public string Address { get; internal set; }
        public string Type { get; set; }
    }
}