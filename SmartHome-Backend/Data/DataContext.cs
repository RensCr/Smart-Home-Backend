using Microsoft.EntityFrameworkCore;
using SmartHome_Backend.Entities;
namespace SmartHome_Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }
        public DbSet<Gebruiker> Gebruikers { get; set; }
    }
    
}
