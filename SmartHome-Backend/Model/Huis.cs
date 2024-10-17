namespace SmartHome_Backend.Model
{
    public class Huis
    {
        public int Id { get; set; }
        public required string Locatie { get; set; }
        public required int GebruikersId { get; set; }
    }
}
