using Syki.Back.CreateCampus;

namespace Syki.Tests.UpdateCampus;

public class UpdateCampusUnitTests
{
    [Test]
    public void Deve_atualizar_os_dados_do_campus_corretamente()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Agreste II";
        const string cidade = "Bonito - PE";

        var campus = new Campus(faculdadeId, "Agreste I", "Caruaru - PE");

        // Act
        campus.Update(nome, cidade);

        // Assert
        campus.Name.Should().Be(nome);
        campus.City.Should().Be(cidade);
    }

    [Test]
    public void Deve_converter_o_campus_corretamente_pro_out()
    {
        // Arrange
        var campus = new Campus(Guid.NewGuid(), "Agreste II", "Bonito - PE");

        // Act
        var campusOut = campus.ToUpdateCampusOut();

        // Assert
        campusOut.Id.Should().Be(campus.Id);
        campusOut.Name.Should().Be(campus.Name);
        campusOut.City.Should().Be(campus.City);
    }
}
