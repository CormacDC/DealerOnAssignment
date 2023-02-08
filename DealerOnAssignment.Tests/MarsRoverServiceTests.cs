using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Numerics;
using Xunit.Sdk;

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
}