using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_turma()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados" });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        var body = new TurmaIn(disciplina.Id, professor.Id, periodo.Id, horarios);

        // Act
        var turma = await client.PostAsync<TurmaOut>("/turmas", body);

        // Assert
        turma.Id.Should().NotBeEmpty();
        turma.Disciplina.Should().Be(disciplina.Nome);
        turma.Professor.Should().Be(professor.Nome);
        turma.Periodo.Should().Be(periodo.Id);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_disciplina()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        var body = new TurmaIn(Guid.NewGuid(), Guid.NewGuid(), "2024.1", horarios);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        await response.AssertBadRequest(Throw.DE004);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_professor()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        var body = new TurmaIn { DisciplinaId = disciplina.Id, Periodo = periodo.Id, Horarios = horarios };

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        await response.AssertBadRequest(Throw.DE018);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados" });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        var body = new TurmaIn(disciplina.Id, professor.Id, "2024.1", horarios);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_com_horario_invalido()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados" });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H07_00) };

        var body = new TurmaIn(disciplina.Id, professor.Id, periodo.Id, horarios);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        await response.AssertBadRequest(Throw.DE021);
    }

    [Test]
    public async Task Deve_retornar_todas_as_turmas_da_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados" });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };

        var body = new TurmaIn(disciplina.Id, professor.Id, periodo.Id, horarios);
        var turma = await client.PostAsync<TurmaOut>("/turmas", body);

        // Act
        var turmas = await client.GetAsync<List<TurmaOut>>("/turmas");

        // Assert
        turmas.Count.Should().Be(1);
        turmas[0].Id.Should().Be(turma.Id);
    }
}
