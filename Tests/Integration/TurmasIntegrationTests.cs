using System.Net;
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
        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var bodyProfessor = new ProfessorIn { Nome = "Chico" };
        var professor = await client.PostAsync<ProfessorOut>("/professores", bodyProfessor);

        var bodyPeriodo = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", bodyPeriodo);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, ProfessorId = professor.Id, Periodo = periodo.Id };

        // Act
        var turma = await client.PostAsync<TurmaOut>("/turmas", body);

        // Assert
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
        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new TurmaIn { DisciplinaId = Guid.NewGuid(), Periodo = "2024.1" };

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0002);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_professor()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, Periodo = "2024.1" };

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0015);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var bodyProfessor = new ProfessorIn { Nome = "Chico" };
        var professor = await client.PostAsync<ProfessorOut>("/professores", bodyProfessor);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, ProfessorId = professor.Id, Periodo = "2024.1" };

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0003);       
    }

    [Test]
    public async Task Deve_retornar_todas_as_turmas_da_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var bodyProfessor = new ProfessorIn { Nome = "Chico" };
        var professor = await client.PostAsync<ProfessorOut>("/professores", bodyProfessor);

        var bodyPeriodo = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", bodyPeriodo);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, ProfessorId = professor.Id, Periodo = periodo.Id };
        await client.PostAsync<TurmaOut>("/turmas", body);

        // Act
        var turmas = await client.GetAsync<List<TurmaOut>>("/turmas");

        // Assert
        turmas.Count.Should().Be(1);
    }
}
