namespace SmartHome_Backend.Entities
{
    public class Huis
    {
        public int Id { get; set; }
        public required string Locatie { get; set; }
        public required int GebruikersId { get; set; }
    }
}
