using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateDiscipline;
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
        grade.CourseId.Should().Be(cursoId);
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
            Curso = new Course(institutionId, "ADS", CourseType.Bacharelado),
            Disciplines = [
                new Discipline(institutionId, "Banco de Dados"),
                new Discipline(institutionId, "Estrutura de Dados"),
            ],
        };

        grade.Links.Add(new(grade.Disciplines[0].Id, 2, 12, 80));
        grade.Links.Add(new(grade.Disciplines[1].Id, 1, 8, 50));

        // Act
        var gradeOut = grade.ToOut();

        // Assert
        gradeOut.Id.Should().Be(grade.Id);
        gradeOut.CourseId.Should().Be(grade.CourseId);
        gradeOut.CursoNome.Should().Be(grade.Curso.Name);
        gradeOut.Name.Should().Be(grade.Name);

        gradeOut.Disciplines.Should().HaveCount(2);
        gradeOut.Disciplines[0].Name.Should().Be("Banco de Dados");
        gradeOut.Disciplines[0].Period.Should().Be(2);
        gradeOut.Disciplines[0].Credits.Should().Be(12);
        gradeOut.Disciplines[0].Workload.Should().Be(80);
        gradeOut.Disciplines[1].Name.Should().Be("Estrutura de Dados");
        gradeOut.Disciplines[1].Period.Should().Be(1);
        gradeOut.Disciplines[1].Credits.Should().Be(8);
        gradeOut.Disciplines[1].Workload.Should().Be(50);
    }
}
