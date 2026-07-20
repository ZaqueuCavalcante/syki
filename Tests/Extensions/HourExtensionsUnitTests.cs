namespace Estud.Tests.Extensions;

public class HourExtensionsUnitTests
{
    [Test]
    [TestCase(Hour.H00_00, 0)]
    [TestCase(Hour.H07_00, 420)]
    [TestCase(Hour.H07_15, 435)]
    [TestCase(Hour.H07_30, 450)]
    [TestCase(Hour.H07_45, 465)]
    [TestCase(Hour.H12_00, 720)]
    [TestCase(Hour.H13_45, 825)]
    [TestCase(Hour.H18_30, 1110)]
    [TestCase(Hour.H23_45, 1425)]
    public void HourExtensions_Should_convert_hour_to_minutes(Hour hour, int expected)
    {
        // Arrange / Act
        var result = hour.ToMinutes();

        // Assert
        result.Should().Be(expected);
    }
}
