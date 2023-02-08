using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        string gridSize, float expectedX, float expectedY)
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
    public void ConvertStringBearingToVector_ValidNorthInput_CorrectVectorOut(
        string bearing, float expectedX, float expectedY, float expectedZ)
    {
        var bearingVector = _MarsRoverService.ConvertStringBearingToVector(bearing);

        Assert.True(Math.Abs(bearingVector.X - expectedX) < 0.001);
        Assert.True(Math.Abs(bearingVector.Y - expectedY) < 0.001);
        Assert.True(Math.Abs(bearingVector.Z - expectedZ) < 0.001);
    }
}