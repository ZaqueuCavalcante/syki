using Syki.Shared;
using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class GradesUnitTests
{
    [Test]
    public void Deve_criar_uma_grade_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var grade = new Grade(faculdadeId, Guid.NewGuid(), "Grade de ADS - 1.0");

        // Assert
        grade.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_grade_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();

        // Act
        var grade = new Grade(faculdadeId, Guid.NewGuid(), "Grade de ADS - 1.0");

        // Assert
        grade.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_uma_grade_com_curso_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();

        // Act
        var grade = new Grade(faculdadeId, cursoId, "Grade de ADS - 1.0");

        // Assert
        grade.CursoId.Should().Be(cursoId);
    }

    [Test]
    public void Deve_criar_uma_grade_com_nome_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        const string nome = "Grade de ADS - 1.0";

        // Act
        var grade = new Grade(faculdadeId, Guid.NewGuid(), nome);

        // Assert
        grade.Nome.Should().Be(nome);
    }

    [Test]
    public void Deve_converter_a_grade_corretamente_pro_out()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var cursoId = Guid.NewGuid();
        const string nome = "Grade de ADS - 1.0";

        var grade = new Grade(faculdadeId, cursoId, nome)
        {
            Curso = new Curso(faculdadeId, "ADS", TipoDeCurso.Bacharelado),
            Disciplinas = [
                new Disciplina(faculdadeId, "Banco de Dados", 72),
                new Disciplina(faculdadeId, "Estruturas de Dados", 60),
            ],
            Vinculos = [],
        };

        grade.Vinculos.Add(new()
        {
            DisciplinaId = grade.Disciplinas[0].Id,
            Periodo = 2,
            Creditos = 12,
            CargaHoraria = 80
        });

        grade.Vinculos.Add(new()
        {
            DisciplinaId = grade.Disciplinas[1].Id,
            Periodo = 1,
            Creditos = 8,
            CargaHoraria = 50
        });

        // Act
        var gradeOut = grade.ToOut();

        // Assert
        gradeOut.Id.Should().Be(grade.Id);
        gradeOut.CursoId.Should().Be(grade.CursoId);
        gradeOut.CursoNome.Should().Be(grade.Curso.Nome);
        gradeOut.Nome.Should().Be(grade.Nome);

        gradeOut.Disciplinas.Should().HaveCount(2);
        gradeOut.Disciplinas[0].Nome.Should().Be("Banco de Dados");
        gradeOut.Disciplinas[0].Periodo.Should().Be(2);
        gradeOut.Disciplinas[0].Creditos.Should().Be(12);
        gradeOut.Disciplinas[0].CargaHoraria.Should().Be(80);
        gradeOut.Disciplinas[1].Nome.Should().Be("Estruturas de Dados");
        gradeOut.Disciplinas[1].Periodo.Should().Be(1);
        gradeOut.Disciplinas[1].Creditos.Should().Be(8);
        gradeOut.Disciplinas[1].CargaHoraria.Should().Be(50);
    }
}
