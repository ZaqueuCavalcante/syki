namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_turma()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var disciplina = await client.CreateDisciplina();
        var professor = await client.CreateProfessor();
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        // Act
        var turma = await client.Createturma(disciplina.Id, professor.Id, periodo.Id, horarios);

        // Assert
        turma.Id.Should().NotBeEmpty();
        turma.Disciplina.Should().Be(disciplina.Nome);
        turma.Professor.Should().Be(professor.Nome);
        turma.Periodo.Should().Be(periodo.Id);
        turma.Horarios.Should().ContainSingle();
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_disciplina()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var response = await client.CreateturmaHttp(Guid.NewGuid(), Guid.NewGuid(), "2024.1", []);

        // Assert
        await response.AssertBadRequest(Throw.DE004);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_professor()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var disciplina = await client.CreateDisciplina();

        // Act
        var response = await client.CreateturmaHttp(disciplina.Id, Guid.NewGuid(), "2024.1", []);

        // Assert
        await response.AssertBadRequest(Throw.DE018);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var disciplina = await client.CreateDisciplina();
        var professor = await client.CreateProfessor();
        
        // Act
        var response = await client.CreateturmaHttp(disciplina.Id, professor.Id, "2024.1", []);

        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_com_horario_invalido()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var disciplina = await client.CreateDisciplina();
        var professor = await client.CreateProfessor();
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H07_00) };

        // Act
        var response = await client.CreateturmaHttp(disciplina.Id, professor.Id, periodo.Id, horarios);

        // Assert
        await response.AssertBadRequest(Throw.DE021);
    }

    [Test]
    public async Task Deve_retornar_todas_as_turmas_da_institution()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var disciplina = await client.CreateDisciplina();
        var professor = await client.CreateProfessor();
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        var turma = await client.Createturma(disciplina.Id, professor.Id, periodo.Id, horarios);

        // Act
        var turmas = await client.GetAsync<List<TurmaOut>>("/turmas");

        // Assert
        turmas.Count.Should().Be(1);
        turmas[0].Id.Should().Be(turma.Id);
    }
}
