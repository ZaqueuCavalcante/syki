using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class FaculdadesUnitTests
{
    [Test]
    public void Deve_criar_uma_faculdade_com_id()
    {
        // Arrange
        const string nome = "UFPE";

        // Act
        var faculdade = new Faculdade(nome);

        // Assert
        faculdade.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_faculdade_com_nome_correto()
    {
        // Arrange
        const string nome = "UFPE";

        // Act
        var faculdade = new Faculdade(nome);

        // Assert
        faculdade.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_converter_o_aluno_corretamente_pro_out()
    {
        // Arrange
        var faculdade = new Faculdade("UFPE");

        // Act
        var faculdadeOut = faculdade.ToOut();

        // Assert
        faculdadeOut.Id.Should().Be(faculdade.Id);
        faculdadeOut.Nome.Should().Be(faculdade.Nome);
    }
}
