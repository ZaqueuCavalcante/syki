using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class TurmasIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_uma_turma()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var bodyProfessor = new ProfessorIn { Nome = "Chico" };
        var professor = await PostAsync<ProfessorOut>("/professores", bodyProfessor);

        var bodyPeriodo = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodo = await PostAsync<PeriodoOut>("/periodos", bodyPeriodo);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, ProfessorId = professor.Id, Periodo = periodo.Id };

        // Act
        var turma = await PostAsync<TurmaOut>("/turmas", body);

        // Assert
        turma.Disciplina.Should().Be(disciplina.Nome);
        turma.Professor.Should().Be(professor.Nome);
        turma.Periodo.Should().Be(periodo.Id);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_disciplina()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new TurmaIn { DisciplinaId = Guid.NewGuid(), Periodo = "2024.1" };

        // Act
        var response = await _client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0002);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_professor()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, Periodo = "2024.1" };

        // Act
        var response = await _client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0015);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_periodo()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var bodyProfessor = new ProfessorIn { Nome = "Chico" };
        var professor = await PostAsync<ProfessorOut>("/professores", bodyProfessor);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, ProfessorId = professor.Id, Periodo = "2024.1" };

        // Act
        var response = await _client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0003);       
    }

    [Test]
    public async Task Deve_retornar_todas_as_turmas_da_faculdade()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var bodyDisciplina = new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72 };
        var disciplina = await PostAsync<DisciplinaOut>("/disciplinas", bodyDisciplina);

        var bodyProfessor = new ProfessorIn { Nome = "Chico" };
        var professor = await PostAsync<ProfessorOut>("/professores", bodyProfessor);

        var bodyPeriodo = new PeriodoIn { Id = "2023.1", Start = new DateOnly(2023, 02, 01), End = new DateOnly(2023, 06, 01) };
        var periodo = await PostAsync<PeriodoOut>("/periodos", bodyPeriodo);

        var body = new TurmaIn { DisciplinaId = disciplina.Id, ProfessorId = professor.Id, Periodo = periodo.Id };
        await PostAsync<TurmaOut>("/turmas", body);

        // Act
        var turmas = await GetAsync<List<TurmaOut>>("/turmas");

        // Assert
        turmas.Count.Should().Be(1);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_criar_uma_turma_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        var body = new TurmaIn { Periodo = "2024.1" };

        // Act
        var response = await _client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAcademico))]
    public async Task Nao_deve_buscar_as_turmas_quando_o_usuario_nao_tem_permissao(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        await RegisterAndLogin(faculdade.Id, role);

        // Act
        var response = await _client.GetAsync("/turmas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
