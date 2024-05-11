using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Tests.Features.Academic.CreateCampus;

public class CreateCampusUnitTests
{
    [Test]
    public void Should_create_a_campus_with_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var campus = new Campus(institutionId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Should_create_a_campus_with_correct_institution_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var campus = new Campus(institutionId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Should_create_a_campus_with_correct_name()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Agreste I";

        // Act
        var campus = new Campus(institutionId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.Name.Should().Be(name);
    }

    [Test]
    public void Should_create_a_campus_with_correct_city()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string cidade = "Caruaru - PE";

        // Act
        var campus = new Campus(institutionId, "Agreste I", cidade);

        // Assert
        campus.City.Should().Be(cidade);
    }

    [Test]
    public void Should_convert_campus_to_out()
    {
        // Arrange
        var campus = new Campus(Guid.NewGuid(), "Agreste II", "Bonito - PE");

        // Act
        var campusOut = campus.ToOut();

        // Assert
        campusOut.Id.Should().Be(campus.Id);
        campusOut.Name.Should().Be(campus.Name);
        campusOut.City.Should().Be(campus.City);
    }
}
