using Syki.Back.CreateProfessor;
using Syki.Back.Features.Academico.CreateDisciplina;

namespace Syki.Tests.Unit;

public class TurmasUnitTests
{
    [Test]
    public void Deve_criar_uma_turma_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, []);

        // Assert
        turma.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_turma_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, []);

        // Assert
        turma.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_disciplina_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, []);

        // Assert
        turma.DisciplinaId.Should().Be(disciplinaId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_professor_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, []);

        // Assert
        turma.ProfessorId.Should().Be(professorId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_periodo_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, []);

        // Assert
        turma.Periodo.Should().Be(periodo);
    }

    [Test]
    public void Deve_criar_uma_turma_com_horarios_corretos()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";
        var horarios = new List<Horario>() { new(Day.Segunda, Hora.H07_00, Hora.H08_00) };

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, horarios);

        // Assert
        turma.Horarios.Should().BeEquivalentTo(horarios);
    }

    [Test]
    public void Nao_deve_criar_uma_turma_com_horarios_totalmente_conflitantes()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";
        var horarios = new List<Horario>()
        {
            new(Day.Segunda, Hora.H07_00, Hora.H08_00),
            new(Day.Segunda, Hora.H07_00, Hora.H08_00),
        };

        // Act
        Action act = () => new Turma(institutionId, disciplinaId, professorId, periodo, horarios);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE022);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.HorariosConflitantes))]
    public void Nao_deve_criar_uma_turma_com_horarios_parcialmente_conflitantes(List<Horario> horarios)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        Action act = () => new Turma(institutionId, disciplinaId, professorId, periodo, horarios);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE022);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.HorariosValidos))]
    public void Deve_criar_uma_turma_com_horarios_validos(List<Horario> horarios)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        // Act
        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, horarios);

        // Assert
        turma.Horarios.Should().BeEquivalentTo(horarios);
    }

    [Test]
    public void Deve_converter_a_turma_corretamente_pro_out()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var disciplinaId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string periodo = "2023.2";

        var disciplina = new Disciplina(institutionId, "Banco de Dados");
        var professor = new Professor(userId, institutionId, "Chico Science");
        var horarios = new List<Horario>() { new(Day.Segunda, Hora.H07_00, Hora.H08_00) };

        var turma = new Turma(institutionId, disciplinaId, professorId, periodo, horarios)
        {
            Disciplina = disciplina,
            Professor = professor,
        };

        // Act
        var turmaOut = turma.ToOut();

        // Assert
        turmaOut.Id.Should().Be(turma.Id);
        turmaOut.Disciplina.Should().Be(turma.Disciplina.Name);
        turmaOut.Professor.Should().Be(turma.Professor.Name);
        turmaOut.Periodo.Should().Be(turma.Periodo);
        turmaOut.Horarios.Should().HaveCount(1);
    }
}
