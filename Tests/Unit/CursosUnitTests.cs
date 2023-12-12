using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;
using static Syki.Shared.TipoDeCurso;

namespace Syki.Tests.Unit;

public class CursosUnitTests
{
    [Test]
    public void Deve_criar_um_curso_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(faculdadeId, nome, Bacharelado);

        // Assert
        curso.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_curso_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(faculdadeId, nome, Bacharelado);

        // Assert
        curso.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_um_curso_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(faculdadeId, nome, Bacharelado);

        // Assert
        curso.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_criar_um_curso_com_tipo_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(faculdadeId, nome, Bacharelado);

        // Assert
        curso.Tipo.Should().Be(Bacharelado);
    }

    [Test]
    public void Deve_converter_corretamente_pro_out()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Análise e Desenvolvimento de Sistemas";
        var curso = new Curso(faculdadeId, nome, Bacharelado);

        // Act
        var cursoOut = curso.ToOut();

        // Assert
        cursoOut.Id.Should().Be(curso.Id);
        cursoOut.Nome.Should().Be(curso.Nome);
        cursoOut.Tipo.Should().Be(curso.Tipo);
    }
}
