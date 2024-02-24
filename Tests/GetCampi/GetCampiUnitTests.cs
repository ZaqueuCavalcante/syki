using Syki.Back.CreateCampus;

namespace Syki.Tests.GetCampi;

public class GetCampiUnitTests
{
    [Test]
    public void Should_convert_campus_to_get_campi_out()
    {
        // Arrange
        var campus = new Campus(Guid.NewGuid(), "Agreste II", "Bonito - PE");

        // Act
        var campusOut = campus.ToGetCampusOut();

        // Assert
        campusOut.Id.Should().Be(campus.Id);
        campusOut.Name.Should().Be(campus.Name);
        campusOut.City.Should().Be(campus.City);
    }
}
