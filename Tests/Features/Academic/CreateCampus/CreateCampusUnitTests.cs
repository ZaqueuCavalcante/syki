using Syki.Back.Features.Academico.CreateCampus;

namespace Syki.Tests.CreateCampus;

public class CreateCampusUnitTests
{
    [Test]
    public void Deve_criar_um_campus_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var campus = new Campus(institutionId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_campus_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var campus = new Campus(institutionId, "Agreste I", "Caruaru - PE");

        // Assert
        campus.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_um_campus_com_nome_correto()
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
    public void Deve_criar_um_campus_com_cidade_correta()
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
    public void Deve_converter_o_campus_corretamente_pro_out()
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
