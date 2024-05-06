using Syki.Back.Features.Academic.CreateCurso;
using Syki.Back.Features.Academic.CreateDisciplina;
using Syki.Back.Features.Academic.CreateGrade;

namespace Syki.Tests.Unit;

public class CreateGradeUnitTests
{
    [Test]
    public void Deve_criar_uma_grade_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var grade = new Grade(institutionId, Guid.NewGuid(), "Grade de ADS - 1.0");

        // Assert
        grade.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_grade_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();

        // Act
        var grade = new Grade(institutionId, Guid.NewGuid(), "Grade de ADS - 1.0");

        // Assert
        grade.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_uma_grade_com_curso_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        // Act
        var grade = new Grade(institutionId, cursoId, "Grade de ADS - 1.0");

        // Assert
        grade.CursoId.Should().Be(cursoId);
    }

    [Test]
    public void Deve_criar_uma_grade_com_nome_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Grade de ADS - 1.0";

        // Act
        var grade = new Grade(institutionId, Guid.NewGuid(), name);

        // Assert
        grade.Name.Should().Be(name);
    }

    [Test]
    public void Deve_converter_a_grade_corretamente_pro_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        const string name = "Grade de ADS - 1.0";

        var grade = new Grade(institutionId, cursoId, name)
        {
            Curso = new Curso(institutionId, "ADS", CourseType.Bacharelado),
            Disciplinas = [
                new Disciplina(institutionId, "Banco de Dados"),
                new Disciplina(institutionId, "Estrutura de Dados"),
            ],
        };

        grade.Vinculos.Add(new(grade.Disciplinas[0].Id, 2, 12, 80));
        grade.Vinculos.Add(new(grade.Disciplinas[1].Id, 1, 8, 50));

        // Act
        var gradeOut = grade.ToOut();

        // Assert
        gradeOut.Id.Should().Be(grade.Id);
        gradeOut.CursoId.Should().Be(grade.CursoId);
        gradeOut.CursoNome.Should().Be(grade.Curso.Name);
        gradeOut.Name.Should().Be(grade.Name);

        gradeOut.Disciplinas.Should().HaveCount(2);
        gradeOut.Disciplinas[0].Name.Should().Be("Banco de Dados");
        gradeOut.Disciplinas[0].Periodo.Should().Be(2);
        gradeOut.Disciplinas[0].Creditos.Should().Be(12);
        gradeOut.Disciplinas[0].CargaHoraria.Should().Be(80);
        gradeOut.Disciplinas[1].Name.Should().Be("Estrutura de Dados");
        gradeOut.Disciplinas[1].Periodo.Should().Be(1);
        gradeOut.Disciplinas[1].Creditos.Should().Be(8);
        gradeOut.Disciplinas[1].CargaHoraria.Should().Be(50);
    }
}
