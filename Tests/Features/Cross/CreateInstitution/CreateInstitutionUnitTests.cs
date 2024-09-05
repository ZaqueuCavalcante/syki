using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Tests.Features.Cross.CreateInstitution;

public class CreateInstitutionUnitTests
{
    [Test]
    public void Should_create_institution_with_correct_data()
    {
        // Arrange
        const string name = "UFPE";

        // Act
        var institution = new Institution(name);

        // Assert
        institution.Id.Should().NotBeEmpty();
        institution.Name.Should().Be(name);
        institution.Configs.InstitutionId.Should().Be(institution.Id);
        institution.Configs.NoteLimit.Should().Be(7.00M);
        institution.Configs.FrequencyLimit.Should().Be(70.00M);
    }

    [Test]
    public void Should_convert_institution_to_out()
    {
        // Arrange
        var institution = new Institution("UFPE");

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
        var institution = new Institution("UFPE");
        var institutionOut = institution.ToOut();

        // Act
        var name = institutionOut.ToString();

        // Assert
        name.Should().Be(institution.Name);
    }
}
