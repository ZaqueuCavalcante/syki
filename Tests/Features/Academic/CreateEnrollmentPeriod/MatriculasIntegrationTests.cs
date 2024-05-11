namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_criar_um_novo_periodo_de_matricula()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var enrollmentPeriod = await client.CreateEnrollmentPeriod(period.Id, "15/01", "28/01");

        // Assert
        enrollmentPeriod.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Deve_retornar_os_periodos_de_matricula()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id, "15/01", "28/01");

        // Act
        var periods = await client.GetEnrollmentPeriods();

        // Assert
        periods.Should().ContainSingle();
    }

    [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_quando_nao_ha_periodo_de_matricula_cadastrado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCurso("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, periodo.Id, Turno.Noturno);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");
        var clientAluno = await _factory.LoggedAsStudent(aluno.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<MatriculaTurmaOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_antes_do_periodo_de_matricula()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCurso("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, periodo.Id, Turno.Noturno);

        var year = DateTime.Now.Year;
        var period = await client.CreateAcademicPeriod($"{year}.1");

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");
        var clientAluno = await _factory.LoggedAsStudent(aluno.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<MatriculaTurmaOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test]
    public async Task Nao_deve_retornar_nenhuma_turma_depois_do_periodo_de_matricula()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCurso("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, periodo.Id, Turno.Noturno);

        var year = DateTime.Now.Year;
        var period = await client.CreateAcademicPeriod($"{year}.1");

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-4));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");
        var clientAluno = await _factory.LoggedAsStudent(aluno.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<MatriculaTurmaOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test]
    public async Task Deve_retornar_apenas_as_turmas_cujas_disciplinas_estao_na_grade_da_oferta_de_curso_do_aluno()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var year = DateTime.Now.Year;
        var periodo = await client.CreateAcademicPeriod($"{year}.1");
        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        await client.CreateEnrollmentPeriod(periodo.Id, start, end);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var ads = await client.CreateCurso("ADS");
        var direito = await client.CreateCurso("Direito");

        var matematica = await client.CreateDisciplina("Matemática Discreta", [ads.Id]);
        var bancoDeDados = await client.CreateDisciplina("Banco de Dados", [ads.Id]);
        var estruturaDeDados = await client.CreateDisciplina("Estrutura de Dados", [ads.Id]);
        var infoSociedade = await client.CreateDisciplina("Informática e Sociedade", [ads.Id, direito.Id]);
        var direitoEconomia = await client.CreateDisciplina("Direito e Economia", [direito.Id]);
        var introDireito = await client.CreateDisciplina("Introdução ao Direito", [direito.Id]);
        var direitoFinanceiro = await client.CreateDisciplina("Direito Financeiro", [direito.Id]);

        var gradeAds = await client.CreateGrade("Grade ADS 1.0", ads.Id,
        [
            new(matematica.Id, 1, 7, 73),
            new(bancoDeDados.Id, 1, 7, 73),
            new(estruturaDeDados.Id, 2, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
        ]);

        var gradeDireito = await client.CreateGrade("Grade Direito 1.0", direito.Id,
        [
            new(direitoEconomia.Id, 1, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
            new(introDireito.Id, 1, 7, 73),
            new(direitoFinanceiro.Id, 1, 7, 73),
        ]);

        var ofertaAds = await client.CreateOferta(campus.Id, ads.Id, gradeAds.Id, periodo.Id, Turno.Noturno);
        var ofertaDireito = await client.CreateOferta(campus.Id, direito.Id, gradeDireito.Id, periodo.Id, Turno.Noturno);

        var chico = await client.CreateProfessor("Chico");
        var ana = await client.CreateProfessor("Ana");

        var turmaMatematica = await client.Createturma(matematica.Id, chico.Id, periodo.Id, [new(Day.Segunda, Hora.H07_00, Hora.H10_00)]);
        var turmaBancoDeDados = await client.Createturma(bancoDeDados.Id, chico.Id, periodo.Id, [new(Day.Terca, Hora.H07_00, Hora.H10_00)]);
        var turmaEstruturaDeDados = await client.Createturma(estruturaDeDados.Id, chico.Id, periodo.Id, [new(Day.Quarta, Hora.H07_00, Hora.H10_00)]);
        var turmaInfoSociedade = await client.Createturma(infoSociedade.Id, ana.Id, periodo.Id, [new(Day.Segunda, Hora.H07_00, Hora.H08_00)]);
        var turmaDireitoEconomia = await client.Createturma(direitoEconomia.Id, ana.Id, periodo.Id, [new(Day.Terca, Hora.H07_00, Hora.H08_00)]);
        var turmaIntroDireito = await client.Createturma(introDireito.Id, ana.Id, periodo.Id, [new(Day.Quarta, Hora.H07_00, Hora.H08_00)]);
        var turmaDireitoFinanceiro = await client.Createturma(direitoFinanceiro.Id, ana.Id, periodo.Id, [new(Day.Quinta, Hora.H07_00, Hora.H08_00)]);

        var zaqueu = await client.CreateStudent(ofertaAds.Id, "Zaqueu");
        var maju = await client.CreateStudent(ofertaDireito.Id, "Maju");

        var clientAluno = await _factory.LoggedAsStudent(zaqueu.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<MatriculaTurmaOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().HaveCount(4);
        turmas.Should().Contain(t => t.Id == turmaMatematica.Id);
        turmas.Should().Contain(t => t.Id == turmaBancoDeDados.Id);
        turmas.Should().Contain(t => t.Id == turmaEstruturaDeDados.Id);
        turmas.Should().Contain(t => t.Id == turmaInfoSociedade.Id);
    }
}
