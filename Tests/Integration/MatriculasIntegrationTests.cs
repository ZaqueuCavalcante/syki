using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var body = new PeriodoDeMatriculaIn(periodo.Id, "15/01", "28/01");

        // Act
        var periodoDeMatricula = await client.PostAsync<PeriodoDeMatriculaOut>("/matriculas/periodos", body);

        // Assert
        periodoDeMatricula.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Deve_retornar_os_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn("2024.1"));
        var body = new PeriodoDeMatriculaIn(periodo.Id, "20/01", "30/12");
        await client.PostAsync<PeriodoDeMatriculaOut>("/matriculas/periodos", body);

        // Act
        var periodos = await client.GetAsync<List<PeriodoDeMatriculaOut>>("/matriculas/periodos");

        // Assert
        periodos.Should().ContainSingle();
    }

    [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_quando_nao_ha_periodo_de_matricula_cadastrado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_antes_do_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var year = DateTime.Now.Year;
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn($"{year}.1"));

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
        await client.PostAsync<PeriodoDeMatriculaOut>("/matriculas/periodos", new PeriodoDeMatriculaIn { Id = periodo.Id, Start = start, End = end });

        await client.LoginAsAdm();
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_depois_do_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var year = DateTime.Now.Year;
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn($"{year}.1"));

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-4));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        await client.PostAsync<PeriodoDeMatriculaOut>("/matriculas/periodos", new PeriodoDeMatriculaIn { Id = periodo.Id, Start = start, End = end });

        await client.LoginAsAdm();
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test]
    public async Task Deve_retornar_as_turmas_disponiveis_para_o_aluno_escolher_na_sua_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var year = DateTime.Now.Year;
        var periodo = await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn($"{year}.1"));

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        await client.PostAsync<PeriodoDeMatriculaOut>("/matriculas/periodos", new PeriodoDeMatriculaIn { Id = periodo.Id, Start = start, End = end });

        var campus = await client.PostAsync<CampusOut>("/campi", new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" });
        var curso = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var disciplina = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", CargaHoraria = 72, Cursos = [curso.Id] });
        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = curso.Id, Disciplinas = [new(disciplina.Id, 1, 7, 73)] });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id, });

        var professor = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        var horarios = new List<HorarioIn>() { new(Dia.Segunda, Hora.H07_00, Hora.H08_00) };
        var turma = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(disciplina.Id, professor.Id, periodo.Id, horarios));

        var aluno = await client.PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id });
        var password = await client.ResetPassword(aluno.UserId);
        await client.Login(aluno.Email, password);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().ContainSingle();
        turmas[0].Id.Should().Be(turma.Id);
        turmas[0].Disciplina.Should().Be(disciplina.Nome);
        turmas[0].Periodo.Should().Be(1);
        turmas[0].Creditos.Should().Be(7);
        turmas[0].CargaHoraria.Should().Be(73);
        turmas[0].Professor.Should().Be(professor.Nome);
        turmas[0].Horario.Should().Be(turma.Horario);
    }
}
