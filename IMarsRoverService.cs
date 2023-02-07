using System.Numerics;

namespace DealerOnAssignment
{
    public interface IMarsRoverService
    {
        void Run(string fileName);
        Vector2 ConvertGridSizeToVector(string gridSize);
        Vector3 ConvertStringBearingToVector(string bearing);
        Vector3 GetFinalBearing(Vector2 gridSizeVector, Vector3 initialBearing, string directions);
        Vector3 MoveRover(Vector3 currentBearing, Vector2 gridSizeVector);
        void LogFinalBearing(Vector3 finalBearing, string outputFileName);
        string CreateFinalBearingMessage(Vector3 finalBearing);
    }
}