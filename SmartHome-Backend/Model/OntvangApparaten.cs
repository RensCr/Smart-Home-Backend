namespace SmartHome_Backend.Model
{
    public class OntvangApparaten
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public bool Slim { get; set; }
        public string apparaatType { get; set; }
        public int HuisId { get; set; }
        public bool Status { get; set; }
    }
}
