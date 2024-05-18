namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Deve_criar_uma_turma()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var professor = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        // Act
        var turma = await client.CreateClass(discipline.Id, professor.Id, period.Id, schedules);

        // Assert
        turma.Id.Should().NotBeEmpty();
        turma.Discipline.Should().Be(discipline.Name);
        turma.Teacher.Should().Be(professor.Name);
        turma.Period.Should().Be(period.Id);
        turma.Schedules.Should().ContainSingle();
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_discipline()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.CreateClassHttp(Guid.NewGuid(), Guid.NewGuid(), "2024.1", []);

        // Assert
        await response.AssertBadRequest(Throw.DE004);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_professor()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();

        // Act
        var response = await client.CreateClassHttp(discipline.Id, Guid.NewGuid(), "2024.1", []);

        // Assert
        await response.AssertBadRequest(Throw.DE018);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var professor = await client.CreateTeacher();
        
        // Act
        var response = await client.CreateClassHttp(discipline.Id, professor.Id, "2024.1", []);

        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_turma_com_schedule_invalido()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var professor = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H07_00) };

        // Act
        var response = await client.CreateClassHttp(discipline.Id, professor.Id, period.Id, schedules);

        // Assert
        await response.AssertBadRequest(Throw.DE021);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_todas_as_turmas_da_institution()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var professor = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        var turma = await client.CreateClass(discipline.Id, professor.Id, period.Id, schedules);

        // Act
        var turmas = await client.GetAsync<List<ClassOut>>("/turmas");

        // Assert
        turmas.Count.Should().Be(1);
        turmas[0].Id.Should().Be(turma.Id);
    }
}
