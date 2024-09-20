namespace SmartHome_Backend.Repositories
{
    public class UserRepository
    {
        private readonly string _ConnectionString;
        public UserRepository(string connectionString)
        {
            _ConnectionString = connectionString;
        }
    }
}
