using Syki.Shared;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_criar_um_novo_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var enrollmentPeriod = await client.CreateEnrollmentPeriod(period.Id, "15/01", "28/01");

        // Assert
        enrollmentPeriod.Id.Should().NotBeEmpty();
    }

    // [Test]
    public async Task Deve_retornar_os_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id, "15/01", "28/01");

        // Act
        var periods = await client.GetEnrollmentPeriods();

        // Assert
        periods.Should().ContainSingle();
    }

    // [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_quando_nao_ha_periodo_de_matricula_cadastrado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    // [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_antes_do_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var year = DateTime.Now.Year;
        var period = await client.CreateAcademicPeriod($"{year}.1");

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        await client.LoginAsAdm();
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    // [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_depois_do_periodo_de_matricula()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var year = DateTime.Now.Year;
        var periodo = await client.CreateAcademicPeriod($"{year}.1");

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-4));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        await client.CreateEnrollmentPeriod(periodo.Id, start, end);

        await client.LoginAsAdm();
        await client.RegisterAndLogin(faculdade.Id, Aluno);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    // [Test]
    public async Task Deve_retornar_apenas_as_turmas_cujas_disciplinas_estao_na_grade_da_oferta_de_curso_do_aluno()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var year = DateTime.Now.Year;
        var periodo = await client.CreateAcademicPeriod($"{year}.1");
        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        await client.CreateEnrollmentPeriod(periodo.Id, start, end);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var ads = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "ADS", Tipo = TipoDeCurso.Bacharelado });
        var direito = await client.PostAsync<CursoOut>("/cursos", new CursoIn { Nome = "Direito", Tipo = TipoDeCurso.Bacharelado });

        var matematica = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Matemática Discreta", Cursos = [ads.Id] });
        var bancoDeDados = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Banco de Dados", Cursos = [ads.Id] });
        var estruturaDeDados = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Estrutura de Dados", Cursos = [ads.Id] });
        var infoSociedade = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Informática e Sociedade", Cursos = [ads.Id, direito.Id] });
        var direitoEconomia = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Direito e Economia", Cursos = [direito.Id] });
        var introDireito = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Introdução ao Direito", Cursos = [direito.Id] });
        var direitoFinanceiro = await client.PostAsync<DisciplinaOut>("/disciplinas", new DisciplinaIn { Nome = "Direito Financeiro", Cursos = [direito.Id] });

        var gradeAds = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = ads.Id, Disciplinas =
        [
            new(matematica.Id, 1, 7, 73),
            new(bancoDeDados.Id, 1, 7, 73),
            new(estruturaDeDados.Id, 2, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
        ]});
        var gradeDireito = await client.PostAsync<GradeOut>("/grades", new GradeIn { Nome = "Grade de ADS - 1.0", CursoId = direito.Id, Disciplinas =
        [
            new(direitoEconomia.Id, 1, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
            new(introDireito.Id, 1, 7, 73),
            new(direitoFinanceiro.Id, 1, 7, 73),
        ]});

        var ofertaAds = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = ads.Id, GradeId = gradeAds.Id, });
        var ofertaDireito = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = direito.Id, GradeId = gradeDireito.Id, });

        var chico = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Chico", Email = TestData.Email });
        var ana = await client.PostAsync<ProfessorOut>("/professores", new ProfessorIn { Nome = "Ana", Email = TestData.Email });

        var turmaMatematica = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(matematica.Id, chico.Id, periodo.Id, [new(Dia.Segunda, Hora.H07_00, Hora.H10_00)]));
        var turmaBancoDeDados = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(bancoDeDados.Id, chico.Id, periodo.Id, [new(Dia.Terca, Hora.H07_00, Hora.H10_00)]));
        var turmaEstruturaDeDados = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(estruturaDeDados.Id, chico.Id, periodo.Id, [new(Dia.Quarta, Hora.H07_00, Hora.H10_00)]));
        var turmaInfoSociedade = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(infoSociedade.Id, ana.Id, periodo.Id, [new(Dia.Segunda, Hora.H07_00, Hora.H08_00)]));
        var turmaDireitoEconomia = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(direitoEconomia.Id, ana.Id, periodo.Id, [new(Dia.Terca, Hora.H07_00, Hora.H08_00)]));
        var turmaIntroDireito = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(introDireito.Id, ana.Id, periodo.Id, [new(Dia.Quarta, Hora.H07_00, Hora.H08_00)]));
        var turmaDireitoFinanceiro = await client.PostAsync<TurmaOut>("/turmas", new TurmaIn(direitoFinanceiro.Id, ana.Id, periodo.Id, [new(Dia.Quinta, Hora.H07_00, Hora.H08_00)]));

        var zaqueu = await client.PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Zaqueu", Email = TestData.Email, OfertaId = ofertaAds.Id });
        var maju = await client.PostAsync<AlunoOut>("/alunos", new AlunoIn { Nome = "Maju", Email = TestData.Email, OfertaId = ofertaDireito.Id });

        var token = await _factory.GetResetPasswordToken(zaqueu.Email);
        var password = await client.ResetPassword(token!);
        await client.Login(zaqueu.Email, password);

        // Act
        var turmas = await client.GetAsync<List<MatriculaTurmaOut>>("/matriculas/turmas");

        // Assert
        turmas.Should().HaveCount(4);
        turmas.Should().Contain(t => t.Id == turmaMatematica.Id);
        turmas.Should().Contain(t => t.Id == turmaBancoDeDados.Id);
        turmas.Should().Contain(t => t.Id == turmaEstruturaDeDados.Id);
        turmas.Should().Contain(t => t.Id == turmaInfoSociedade.Id);
    }
}
