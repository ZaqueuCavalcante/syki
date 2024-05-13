using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Tests.Features.Academic.CreateClass;

public class TurmasUnitTests
{
    [Test]
    public void Deve_criar_uma_turma_com_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, []);

        // Assert
        turma.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_uma_turma_com_institution_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, []);

        // Assert
        turma.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_discipline_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, []);

        // Assert
        turma.DisciplineId.Should().Be(disciplineId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_professor_id_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, []);

        // Assert
        turma.TeacherId.Should().Be(professorId);
    }

    [Test]
    public void Deve_criar_uma_turma_com_periodo_correto()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, []);

        // Assert
        turma.Period.Should().Be(period);
    }

    [Test]
    public void Deve_criar_uma_turma_com_schedules_corretos()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";
        var schedules = new List<Schedule>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, schedules);

        // Assert
        turma.Schedules.Should().BeEquivalentTo(schedules);
    }

    [Test]
    public void Nao_deve_criar_uma_turma_com_schedules_totalmente_conflitantes()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";
        var schedules = new List<Schedule>()
        {
            new(Day.Segunda, Hour.H07_00, Hour.H08_00),
            new(Day.Segunda, Hour.H07_00, Hour.H08_00),
        };

        // Act
        Action act = () => new Class(institutionId, disciplineId, professorId, period, schedules);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE022);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.SchedulesConflitantes))]
    public void Nao_deve_criar_uma_turma_com_schedules_parcialmente_conflitantes(List<Schedule> schedules)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        Action act = () => new Class(institutionId, disciplineId, professorId, period, schedules);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE022);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.SchedulesValidos))]
    public void Deve_criar_uma_turma_com_schedules_validos(List<Schedule> schedules)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var turma = new Class(institutionId, disciplineId, professorId, period, schedules);

        // Assert
        turma.Schedules.Should().BeEquivalentTo(schedules);
    }

    [Test]
    public void Deve_converter_a_turma_corretamente_pro_out()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var professorId = Guid.NewGuid();
        const string period = "2023.2";

        var discipline = new Discipline(institutionId, "Banco de Dados");
        var professor = new Teacher(userId, institutionId, "Chico Science");
        var schedules = new List<Schedule>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        var turma = new Class(institutionId, disciplineId, professorId, period, schedules)
        {
            Discipline = discipline,
            Teacher = professor,
        };

        // Act
        var turmaOut = turma.ToOut();

        // Assert
        turmaOut.Id.Should().Be(turma.Id);
        turmaOut.Discipline.Should().Be(turma.Discipline.Name);
        turmaOut.Teacher.Should().Be(turma.Teacher.Name);
        turmaOut.Period.Should().Be(turma.Period);
        turmaOut.Schedules.Should().HaveCount(1);
    }
}
