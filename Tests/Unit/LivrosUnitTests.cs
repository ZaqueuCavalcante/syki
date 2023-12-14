using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class LivrosUnitTests
{
    [Test]
    public void Deve_criar_um_livro_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string titulo = "UFPE";

        // Act
        var livro = new Livro(faculdadeId, titulo);

        // Assert
        livro.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_livro_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string titulo = "UFPE";

        // Act
        var livro = new Livro(faculdadeId, titulo);

        // Assert
        livro.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_livro_com_titulo_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string titulo = "UFPE";

        // Act
        var livro = new Livro(faculdadeId, titulo);

        // Assert
        livro.Titulo.Should().Be(titulo);
    }

    [Test]
    public void Deve_converter_o_livro_corretamente_pro_out()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string titulo = "UFPE";
        var livro = new Livro(faculdadeId, titulo);

        // Act
        var livroOut = livro.ToOut();

        // Assert
        livroOut.Id.Should().Be(livro.Id);
        livroOut.Titulo.Should().Be(livro.Titulo);
    }
}
