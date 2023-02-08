using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace DealerOnAssignment.Tests;

public class MarsRoverServiceTests
{
    private readonly ILogger<MarsRoverService> _log;
    private readonly IConfiguration _config;
    private readonly MarsRoverService _MarsRoverService;

    public MarsRoverServiceTests()
    {
        _log = Mock.Of<ILogger<MarsRoverService>>();
        _config = Mock.Of<IConfiguration>();
        _MarsRoverService = new MarsRoverService(_log, _config);
    }

    [Theory]
    [InlineData("5 5", 5, 5)]
    [InlineData("1 6", 1, 6)]
    [InlineData("1000 1000", 1000, 1000)]
    [InlineData("0 0", 0, 0)]
    public void ConvertGridSizeToVector_ValidInput_CorrectVectorOut(
        string gridSize,
        float expectedX, float expectedY)
    {
        var gridVector = _MarsRoverService.ConvertGridSizeToVector(gridSize);

        Assert.True(Math.Abs(gridVector.X - expectedX) < 0.001);
        Assert.True(Math.Abs(gridVector.Y - expectedY) < 0.001);
    }

    [Theory]
    [InlineData("5 5 N", 5, 5, 4)]
    [InlineData("1 6 E", 1, 6, 5)]
    [InlineData("1000 1000 S", 1000, 1000, 6)]
    [InlineData("0 0 W", 0, 0, 7)]
    public void ConvertStringBearingToVector_ValidInput_CorrectVectorOut(
        string bearing,
        float expectedX, float expectedY, float expectedZ)
    {
        var bearingVector = _MarsRoverService.ConvertStringBearingToVector(bearing);

        Assert.True(Math.Abs(bearingVector.X - expectedX) < 0.001);
        Assert.True(Math.Abs(bearingVector.Y - expectedY) < 0.001);
        Assert.True(Math.Abs(bearingVector.Z - expectedZ) < 0.001);
    }

    [Theory]
    [InlineData(new[] { (float)5, 5 }, new[] { (float)1, 2, 4 }, "LMLMLMLMM", new[] { (float)1, 3, 4 })]
    [InlineData(new[] { (float)5, 5 }, new[] { (float)3, 3, 5 }, "MMRMMRMRRM", new[] { (float)5, 1, 5 })]
    [InlineData(new[] { (float)5, 5 }, new[] { (float)3, 3, 5 }, "MMRMMRMRRMMMM", new[] { (float)5, 1, 5 })]
    [InlineData(new[] { (float)5, 5 }, new[] { (float)3, 3, 5 }, "", new[] { (float)3, 3, 5 })]
    [InlineData(new[] { (float)5, 5 }, new[] { (float)3, 3, 5 }, "LRLRRLRLRLRRRRRRRLLLL", new[] { (float)3, 3, 5 })]
    public void GetFinalBearing_ValidInput_CorrectVectorOut(
        float[] gridSize, float[] initialBearing, string directions, 
        float[] expectedBearing)
    {
        var gridSizeVector = new Vector2(gridSize[0], gridSize[1]);
        var initialBearingVector = new Vector3(initialBearing[0], initialBearing[1], initialBearing[2]);
        
        var finalBearingVector = 
            _MarsRoverService.GetFinalBearing(gridSizeVector, initialBearingVector, directions);

        Assert.True(finalBearingVector.X - expectedBearing[0] < 0.001
                    && finalBearingVector.Y - expectedBearing[1] < 0.001
                    && finalBearingVector.Z - expectedBearing[2] < 0.001);
    }

    [Theory]
    [InlineData(new[] { (float)0, 0, 4 }, new[] { (float)5, 5 }, new[] { (float)0, 1, 4 })]
    [InlineData(new[] { (float)5, 7, 5 }, new[] { (float)10, 10 }, new[] { (float)6, 7, 5 })]
    [InlineData(new[] { (float)4, 10, 6 }, new[] { (float)10, 10 }, new[] { (float)4, 9, 6 })]
    [InlineData(new[] { (float)1, 10, 7 }, new[] { (float)10, 10 }, new[] { (float)0, 10, 7 })]
    [InlineData(new[] { (float)5, 5, 4 }, new[] { (float)5, 5 }, new[] { (float)5, 5, 4 })]
    [InlineData(new[] { (float)5, 5, 5 }, new[] { (float)5, 5 }, new[] { (float)5, 5, 5 })]
    [InlineData(new[] { (float)0, 0, 6 }, new[] { (float)5, 5 }, new[] { (float)0, 0, 6 })]
    [InlineData(new[] { (float)0, 0, 7 }, new[] { (float)5, 5 }, new[] { (float)0, 0, 7 })]
    public void MoveRover_ValidInput_CorrectVectorOut(
        float[] currentBearing, float[] gridSize,
        float[] expectedBearing)
    {
        var currentBearingVector = new Vector3(currentBearing[0], currentBearing[1], currentBearing[2]);
        var gridSizeVector = new Vector2(gridSize[0], gridSize[1]);

        var finalBearingVector = _MarsRoverService.MoveRover(currentBearingVector, gridSizeVector);

        Assert.True(finalBearingVector.X - expectedBearing[0] < 0.001
                    && finalBearingVector.Y - expectedBearing[1] < 0.001
                    && finalBearingVector.Z - expectedBearing[2] < 0.001);
    }

    [Theory]
    [InlineData(new[] { (float)0, 0, 4 }, "0 0 N")]
    [InlineData(new[] { (float)5, 1, 5 }, "5 1 E")]
    [InlineData(new[] { (float)60, 1000, 6 }, "60 1000 S")]
    [InlineData(new[] { (float)9, 9, 7 }, "9 9 W")]
    public void CreateFinalBearingMessage_ValidInput_CorrectStringOutput(
        float[] finalBearing,
        string expectedMessage)
    {
        var finalBearingVector = new Vector3(finalBearing[0], finalBearing[1], finalBearing[2]);

        var message = _MarsRoverService.CreateFinalBearingMessage(finalBearingVector);

        Assert.True(message == expectedMessage);
    }
}