using Syki.Back.Features.Academico.CreateCampus;

namespace Syki.Tests.UpdateCampus;

public class UpdateCampusUnitTests
{
    [Test]
    public void Should_update_campus_data()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Agreste II";
        const string city = "Bonito - PE";

        var campus = new Campus(institutionId, "Agreste I", "Caruaru - PE");

        // Act
        campus.Update(name, city);

        // Assert
        campus.Name.Should().Be(name);
        campus.City.Should().Be(city);
    }
}
