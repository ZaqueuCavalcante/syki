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
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS" });
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ curso.Id ] });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = [ new(disciplina.Id, 1, 10, 70) ], });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });

        var body = new TurmaIn(oferta.Id, disciplina.Id, professor.Id, periodo.Id, Dia.Segunda, Hora.H07_00, Hora.H08_00);

        // Act
        var turma = await client.PostAsync<TurmaOut>("/turmas", body);

        // Assert
        turma.Disciplina.Should().Be(disciplina.Nome);
        turma.Professor.Should().Be(professor.Nome);
        turma.Periodo.Should().Be(periodo.Id);
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_oferta()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new TurmaIn(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2024.1", Dia.Segunda, Hora.H07_00, Hora.H08_00);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0009);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_disciplina()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS" });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var body = new TurmaIn(oferta.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Dia.Segunda, Hora.H07_00, Hora.H08_00);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0002);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_professor()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS" });
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ curso.Id ] });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = [ new(disciplina.Id, 1, 10, 70) ], });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var body = new TurmaIn { OfertaId = oferta.Id, DisciplinaId = disciplina.Id, Periodo = "2024.1" };

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0015);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS" });
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ curso.Id ] });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = [ new(disciplina.Id, 1, 10, 70) ], });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });

        var body = new TurmaIn(oferta.Id, disciplina.Id, professor.Id, "2024.1", Dia.Segunda, Hora.H07_00, Hora.H08_00);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0003);       
    }

    [Test]
    public async Task Nao_deve_criar_uma_turma_sem_horario()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS" });
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ curso.Id ] });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = [ new(disciplina.Id, 1, 10, 70) ], });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });

        var body = new TurmaIn(oferta.Id, disciplina.Id, professor.Id, periodo.Id, Dia.Segunda, Hora.H07_00, Hora.H07_00);

        // Act
        var response = await client.PostAsync("/turmas", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(Throw.DE0018);       
    }

    [Test]
    public async Task Deve_retornar_todas_as_turmas_da_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn { Id = "2023.2", Start = new DateOnly(2023, 07, 01), End = new DateOnly(2023, 12, 01) });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS" });
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [ curso.Id ] });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = [ new(disciplina.Id, 1, 10, 70) ], });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });
        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });

        var body = new TurmaIn(oferta.Id, disciplina.Id, professor.Id, periodo.Id, Dia.Segunda, Hora.H07_00, Hora.H08_00);
        var turma = await client.PostAsync<TurmaOut>("/turmas", body);

        // Act
        var turmas = await client.GetAsync<List<TurmaOut>>("/turmas");

        // Assert
        turmas.Count.Should().Be(1);
    }
}
