using Syki.Back.Features.Academico.CreateCampus;

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
}
