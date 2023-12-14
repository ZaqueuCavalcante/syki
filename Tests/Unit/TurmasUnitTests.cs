using NUnit.Framework;
using Syki.Back.Domain;
using FluentAssertions;

namespace Syki.Tests.Unit;

public class TurmasUnitTests
{
    [Test]
    public void Deve_criar_uma_turma_com_id()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(faculdadeId, disciplinaId, professorId, periodo);

        // Assert
        turma.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_turma_com_faculdade_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(faculdadeId, disciplinaId, professorId, periodo);

        // Assert
        turma.FaculdadeId.Should().Be(faculdadeId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_disciplina_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(faculdadeId, disciplinaId, professorId, periodo);

        // Assert
        turma.DisciplinaId.Should().Be(disciplinaId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_professor_id_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(faculdadeId, disciplinaId, professorId, periodo);

        // Assert
        turma.ProfessorId.Should().Be(professorId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_periodo_correto()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(faculdadeId, disciplinaId, professorId, periodo);

        // Assert
        turma.Periodo.Should().Be(periodo);
    }

    [Test]
    public void Deve_converter_a_turma_corretamente_pro_out()
    {
        // Arrange
        var faculdadeId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        var disciplina = new Disciplina(faculdadeId, "Banco de Dados", 72);
        var professor = new Professor(faculdadeId, "Chico Science");

        var turma = new Turma(faculdadeId, disciplinaId, professorId, periodo)
        {
            Disciplina = disciplina,
            Professor = professor,
        };

        // Act
        var turmaOut = turma.ToOut();

        // Assert
        turmaOut.Disciplina.Should().Be(turma.Disciplina.Nome);
        turmaOut.Professor.Should().Be(turma.Professor.Nome);
        turmaOut.Periodo.Should().Be(turma.Periodo);
    }
}
