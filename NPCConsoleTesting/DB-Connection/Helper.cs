using Microsoft.Extensions.Configuration;

namespace NPCConsoleTesting.DB_Connection
{
    public class Helper
    {
        private readonly IConfiguration _config;
        public Helper(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString()
        {
            return _config.GetSection("ConnectionStrings").GetSection("Default").Value;
        }
    }
}
