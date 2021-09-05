using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NPCConsoleTesting
{
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private readonly IConfiguration _config;

        public GreetingService(ILogger<GreetingService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                //String interpolation used here to log the value of i separately
                _log.LogInformation("Run number {runNumber}", i);
            }
        }
    }
}
