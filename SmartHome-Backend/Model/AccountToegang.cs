namespace SmartHome_Backend.Model
{
    public class AccountToegang
    {
        public string? Naam { get; set; } = null;
        public string Email { get; set; }
        public string Wachtwoord { get; set; }
        public string? WoonPlaats { get; set; } = null;
    }
}
