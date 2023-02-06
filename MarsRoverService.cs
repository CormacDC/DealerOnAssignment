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
            _log.LogInformation("Start moving rovers...");

            // get the text from the input file
            var inputFileName = fileName ?? _config.GetValue<string>("InputFileName")!;

            var input = File.ReadAllText(inputFileName);
            var inputArray = input.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            var gridSizeVector = ConvertGridSizeToVector(inputArray[0]);

            var outputFileName = $"Outputs/output-{DateTime.Now:yyMMdd-hhmmss}.txt";

            // calculate final bearing of each rover
            for (int i = 1; i < inputArray.Length - 1; i += 2)
            {
                var initialBearing = ConvertStringBearingToVector(inputArray[i]);
                Console.WriteLine($"Initial Bearing: {initialBearing.X} {initialBearing.Y} {initialBearing.Z}");
                var directions = inputArray[i + 1];

                var finalBearing = GetFinalBearing(gridSizeVector, initialBearing, directions);

                LogFinalBearing(finalBearing, outputFileName);
            }

            _log.LogInformation("All rovers have finished moving.");
        }

        public Vector2 ConvertGridSizeToVector(string gridSize)
        {
            var gridSizeSplit = gridSize.Split(" ");
            var gridX = float.Parse(gridSizeSplit[0]);
            var gridY = float.Parse(gridSizeSplit[1]);
            return new Vector2(gridX, gridY);
        }

        public Vector3 ConvertStringBearingToVector(string bearing)
        {
            var bearingSplit = bearing.Split(" ");
            var Z = bearingSplit[2] switch
            {
                "N" => 4,
                "E" => 5,
                "S" => 6,
                _ => (float) 7,
            };
            return new Vector3(float.Parse(bearingSplit[0]), float.Parse(bearingSplit[1]), Z);
        }

        public Vector3 GetFinalBearing(Vector2 gridSizeVector, Vector3 initialBearing, string directions)
        {
            var currentBearing = initialBearing;

            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case 'M':
                        currentBearing = MoveRover(currentBearing, gridSizeVector);
                        break;
                    case 'L':
                        currentBearing.Z = (currentBearing.Z - 1) % 4 + 4;
                        break;
                    case 'R':
                        currentBearing.Z = (currentBearing.Z + 1) % 4 + 4;
                        break;
                }
            }

            return currentBearing;
        }

        public Vector3 MoveRover(Vector3 currentBearing, Vector2 gridSizeVector)
        {
            var edgeWarning = 
                "You are at the edge of the plateau and cannot move in the requested direction";

            switch (currentBearing.Z)
            {
                case 4:
                    if (currentBearing.Y < gridSizeVector.Y + 0.5
                        && currentBearing.Y > gridSizeVector.Y - 0.5)
                    {
                        _log.LogWarning(edgeWarning);
                        return currentBearing;
                    }
                    else
                    {
                        Console.WriteLine("Moved 1 space North");
                        return Vector3.Add(currentBearing, new Vector3(0, 1, 0));
                    }
                case 5:
                    if (currentBearing.X < gridSizeVector.X + 0.5
                        && currentBearing.X > gridSizeVector.X - 0.5)
                    {
                        _log.LogWarning(edgeWarning);
                        return currentBearing;
                    }
                    else
                    {
                        Console.WriteLine("Moved 1 space East");
                        return Vector3.Add(currentBearing, new Vector3(1, 0, 0));
                    }
                case 6:
                    if (currentBearing.Y < 0.5 && currentBearing.Y > -0.5)
                    {
                        _log.LogWarning(edgeWarning);
                        return currentBearing;
                    }
                    else
                    {
                        Console.WriteLine("Moved 1 space South");
                        return Vector3.Add(currentBearing, new Vector3(0, -1, 0));
                    }
                default:
                    if (currentBearing.X < 0.5 && currentBearing.X > -0.5)
                    {
                        _log.LogWarning(edgeWarning);
                        return currentBearing;
                    }
                    else
                    {
                        Console.WriteLine("Moved 1 space West");
                        return Vector3.Add(currentBearing, new Vector3(-1, 0, 0));
                    }
            }
        }

        public void LogFinalBearing(Vector3 finalBearing, string outputFileName)
        {
            var finalDirection = finalBearing.Z switch
            {
                4 => "N",
                5 => "E",
                6 => "S",
                _ => "W"
            };

            var finalBearingString = $"{finalBearing.X} {finalBearing.Y} {finalDirection}";

            Console.WriteLine($"Final Bearing: {finalBearingString}");

            using StreamWriter w = File.AppendText(outputFileName);
            w.WriteLine(finalBearingString);
        }
    }
}
