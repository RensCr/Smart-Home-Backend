namespace SmartHome_Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string WachtwoordHash { get; set; }
        public DateTime Aangemaakt { get; set; }
        public DateTime LaatstIngelogd { get; set; }

    }
}
