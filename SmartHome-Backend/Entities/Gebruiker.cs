﻿namespace SmartHome_Backend.Entities
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public required string Naam { get; set; }
        public required string Email { get; set; }
        public required string WachtwoordHash { get; set; }
        public required DateTime Aangemaakt { get; set; }
        public required DateTime LaatstIngelogd { get; set; }
    }
}
