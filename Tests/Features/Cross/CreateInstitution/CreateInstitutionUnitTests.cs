using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Tests.Features.Cross.CreateInstitution;

public class CreateInstitutionUnitTests
{
    [Test]
    public void Should_create_institution_with_correct_data()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        const string name = "UFPE";

        // Act
        var institution = new Institution(id, name);

        // Assert
        institution.Id.Should().Be(id);
        institution.Name.Should().Be(name);
        institution.Configs.InstitutionId.Should().Be(institution.Id);
        institution.Configs.NoteLimit.Should().Be(7.00M);
        institution.Configs.FrequencyLimit.Should().Be(70.00M);
    }

    [Test]
    public void Should_convert_institution_to_out()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        var institution = new Institution(id, "UFPE");

        // Act
        var institutionOut = institution.ToOut();

        // Assert
        institutionOut.Id.Should().Be(institution.Id);
        institutionOut.Name.Should().Be(institution.Name);
        institutionOut.NoteLimit.Should().Be(institution.Configs.NoteLimit);
        institutionOut.FrequencyLimit.Should().Be(institution.Configs.FrequencyLimit);
    }

    [Test]
    public void Should_return_institution_name_on_to_string()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        var institution = new Institution(id, "UFPE");
        var institutionOut = institution.ToOut();

        // Act
        var name = institutionOut.ToString();

        // Assert
        name.Should().Be(institution.Name);
    }
}
