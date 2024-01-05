using Syki.Shared;
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
    public void Deve_converter_o_curso_corretamente_pro_out()
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

    [Test]
    public void Deve_retornar_true_quando_for_o_mesmo_curso()
    {
        // Arrange
        var curso = new Curso(Guid.NewGuid(), "Curso", Bacharelado);
        var cursoOut1 = curso.ToOut();
        var cursoOut2 = curso.ToOut();

        // Act
        var equals = cursoOut1.Equals(cursoOut2);

        // Assert
        equals.Should().BeTrue();
    }

    [Test]
    public void Deve_retornar_false_quando_nao_for_o_mesmo_curso()
    {
        // Arrange
        var curso1 = new Curso(Guid.NewGuid(), "Curso1", Bacharelado);
        var curso2 = new Curso(Guid.NewGuid(), "Curso2", Bacharelado);
        var cursoOut1 = curso1.ToOut();
        var cursoOut2 = curso2.ToOut();

        // Act
        var equals = cursoOut1.Equals(cursoOut2);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Deve_retornar_false_quando_o_outro_curso_for_nulo()
    {
        // Arrange
        var curso = new Curso(Guid.NewGuid(), "Curso1", Bacharelado);
        var cursoOut = curso.ToOut();

        // Act
        var equals = cursoOut.Equals(null);

        // Assert
        equals.Should().BeFalse();
    }

    [Test]
    public void Deve_retornar_o_hash_code_correto()
    {
        // Arrange
        var cursoOut = new CursoOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = cursoOut.GetHashCode();

        // Assert
        hashCode.Should().Be(4523_9002);
    }

    [Test]
    public void Deve_retornar_o_nome_do_curso_como_to_string()
    {
        // Arrange
        var cursoOut = new CursoOut { Nome = "Curso" };

        // Act
        var nome = cursoOut.ToString();

        // Assert
        nome.Should().Be("Curso");
    }
}
