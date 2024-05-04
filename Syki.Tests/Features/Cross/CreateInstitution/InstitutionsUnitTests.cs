using Syki.Back.CreateInstitution;

namespace Syki.Tests.Unit;

public class InstitutionsUnitTests
{
    [Test]
    public void Deve_criar_uma_institution_com_id()
    {
        // Arrange
        const string nome = "UFPE";

        // Act
        var institution = new Institution(nome);

        // Assert
        institution.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_institution_com_nome_correto()
    {
        // Arrange
        const string nome = "UFPE";

        // Act
        var institution = new Institution(nome);

        // Assert
        institution.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out()
    {
        // Arrange
        var institution = new Institution("UFPE");

        // Act
        var institutionOut = institution.ToOut();

        // Assert
        institutionOut.Id.Should().Be(institution.Id);
        institutionOut.Name.Should().Be(institution.Nome);
    }

    [Test]
    public void Deve_retornar_o_nome_da_institution_como_to_string()
    {
        // Arrange
        var institution = new Institution("UFPE");
        var institutionOut = institution.ToOut();

        // Act
        var nome = institutionOut.ToString();

        // Assert
        nome.Should().Be(institution.Nome);
    }
}
