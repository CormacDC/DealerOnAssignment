using Microsoft.Extensions.Logging;

namespace DealerOnAssignment
{
    public class MarsRoverService : IMarsRoverService
    {
        private readonly ILogger<MarsRoverService> _log;

        public MarsRoverService(ILogger<MarsRoverService> log)
        {
            _log = log;
        }

        public void Run(string num)
        {
            Console.WriteLine("test");
            _log.LogInformation("Test {number}", num);
        }
    }
}
