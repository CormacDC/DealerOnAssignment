using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Numerics;

namespace DealerOnAssignment
{
    public class MarsRoverService : IMarsRoverService
    {
        private readonly ILogger<MarsRoverService> _log;
        private readonly IConfiguration _config;

        public MarsRoverService(ILogger<MarsRoverService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void Run(string? fileName)
        {
            Console.WriteLine("test");
            _log.LogInformation("Test {file}", fileName);

            // get the text from the input file
            var inputFileName = fileName ?? _config.GetValue<string>("InputFileName")!;

            var input = File.ReadAllText(inputFileName);
            var inputArray = input.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            // var gridSizeVector = GetGridSizeVector(inputArray[0]);
            var gridSize = inputArray[0].Split(" ");
            var gridX = float.Parse(gridSize[0]);
            var gridY = float.Parse(gridSize[1]);
            var gridSizeVector = new Vector2(gridX, gridY);

            // var outputFileName = GetOutputFileName();

            // calculate final bearing of each rover
            for (int i = 1; i < inputArray.Length - 1; i += 2)
            {
                var initialBearing = inputArray[i];
                var directions = inputArray[i + 1];

                // var finalBearing = GetFinalBearing(initialBearing, directions);

                // outputFinalBearing(finalBearing, outputFileName);
            }
        }
    }
}
