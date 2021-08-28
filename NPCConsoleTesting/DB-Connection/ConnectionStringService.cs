using Microsoft.Extensions.Configuration;

namespace NPCConsoleTesting.DB_Connection
{
    public class ConnectionStringService : IConnectionStringService
    {
        private readonly IConfiguration _config;
        public ConnectionStringService(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString()
        {
            return _config.GetSection("ConnectionStrings").GetSection("Default").Value;
        }
    }
}
