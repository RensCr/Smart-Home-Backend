namespace SmartHome_Backend.Entities
{
    public class Apparaat
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public required bool Slim { get; set; } = false;
        public required int HuisId { get; set; }
        public required int ApparaatTypeId { get; set; }

    }
}
