namespace SmartHome_Backend.Model
{
    public class ApparaatToevoegen
    {
        public int ApparaatTypeId { get; set; }
        public string Naam { get; set; }
        public bool Slim { get; set; }
        public int HuisId { get; set; }
    }
}
