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
    [InlineData("5 5")]
    [InlineData("1 6")]
    [InlineData("1000 1000")]
    [InlineData("0 0")]
    public void ConvertGridSizeToVector_ValidInput_CorrectVectorOut(string gridSize)
    {
        var gridVector = _MarsRoverService.ConvertGridSizeToVector(gridSize);

        var gridSizeSplit = gridSize.Split(' ');
        var expectedX = float.Parse(gridSizeSplit[0].ToString());
        var expectedY = float.Parse(gridSizeSplit[1].ToString());

        Assert.True(Math.Abs(gridVector.X - expectedX) < 0.001);
        Assert.True(Math.Abs(gridVector.Y - expectedY) < 0.001);
    }
}