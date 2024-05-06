using static Syki.Shared.CourseType;
using Syki.Back.Features.Academico.CreateCurso;

namespace Syki.Tests.Unit;

public class CursosUnitTests
{
    [Test]
    public void Deve_criar_um_curso_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(institutionId, name, Bacharelado);

        // Assert
        curso.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_curso_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(institutionId, name, Bacharelado);

        // Assert
        curso.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_um_curso_com_nome_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(institutionId, name, Bacharelado);

        // Assert
        curso.Name.Should().Be(name);
    }

    [Test]
    public void Deve_criar_um_curso_com_tipo_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Análise e Desenvolvimento de Sistemas";

        // Act
        var curso = new Curso(institutionId, name, Bacharelado);

        // Assert
        curso.Type.Should().Be(Bacharelado);
    }

    [Test]
    public void Deve_converter_o_curso_corretamente_pro_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Análise e Desenvolvimento de Sistemas";
        var curso = new Curso(institutionId, name, Bacharelado);

        // Act
        var cursoOut = curso.ToOut();

        // Assert
        cursoOut.Id.Should().Be(curso.Id);
        cursoOut.Name.Should().Be(curso.Name);
        cursoOut.Type.Should().Be(curso.Type);
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
        var cursoOut = new CourseOut { Id = Guid.Parse("ef45239e-0d02-4eb0-b759-47331cfd1a8e") };

        // Act
        var hashCode = cursoOut.GetHashCode();

        // Assert
        hashCode.Should().Be(4523_9002);
    }

    [Test]
    public void Deve_retornar_o_nome_do_curso_como_to_string()
    {
        // Arrange
        var cursoOut = new CourseOut { Name = "Curso" };

        // Act
        var name = cursoOut.ToString();

        // Assert
        name.Should().Be("Curso");
    }
}
