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

    [Test, Ignore("")]
    public async Task Nao_deve_retornar_nenhuma_turma_quando_nao_ha_periodo_de_matricula_cadastrado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var grade = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var oferta = await client.CreateCourseOffering(campus.Id, course.Id, grade.Id, period.Id, Shift.Noturno);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");
        var clientAluno = await _factory.LoggedAsStudent(aluno.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<EnrollmentClassOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test, Ignore("")]
    public async Task Nao_deve_retornar_nenhuma_turma_antes_do_periodo_de_matricula()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var grade = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var oferta = await client.CreateCourseOffering(campus.Id, course.Id, grade.Id, period.Id, Shift.Noturno);

        var year = DateTime.Now.Year;
        var periodo = await client.CreateAcademicPeriod($"{year}.1");

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");
        var clientAluno = await _factory.LoggedAsStudent(aluno.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<EnrollmentClassOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test, Ignore("")]
    public async Task Nao_deve_retornar_nenhuma_turma_depois_do_periodo_de_matricula()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var grade = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var oferta = await client.CreateCourseOffering(campus.Id, course.Id, grade.Id, period.Id, Shift.Noturno);

        var year = DateTime.Now.Year;
        var periodo = await client.CreateAcademicPeriod($"{year}.1");

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-4));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");
        var clientAluno = await _factory.LoggedAsStudent(aluno.Email);

        // Act
        var turmas = await clientAluno.GetAsync<List<EnrollmentClassOut>>("/matriculas/aluno/turmas");

        // Assert
        turmas.Should().BeEmpty();
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_apenas_as_turmas_cujas_disciplines_estao_na_grade_da_oferta_de_curso_do_aluno()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var year = DateTime.Now.Year;
        var period = await client.CreateAcademicPeriod($"{year}.1");
        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var ads = await client.CreateCourse("ADS");
        var direito = await client.CreateCourse("Direito");

        var matematica = await client.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var bancoDeDados = await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        var estruturaDeDados = await client.CreateDiscipline("Estrutura de Dados", [ads.Id]);
        var infoSociedade = await client.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);
        var direitoEconomia = await client.CreateDiscipline("Direito e Economia", [direito.Id]);
        var introDireito = await client.CreateDiscipline("Introdução ao Direito", [direito.Id]);
        var direitoFinanceiro = await client.CreateDiscipline("Direito Financeiro", [direito.Id]);

        var gradeAds = await client.CreateCourseCurriculum("Grade ADS 1.0", ads.Id,
        [
            new(matematica.Id, 1, 7, 73),
            new(bancoDeDados.Id, 1, 7, 73),
            new(estruturaDeDados.Id, 2, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
        ]);

        var gradeDireito = await client.CreateCourseCurriculum("Grade Direito 1.0", direito.Id,
        [
            new(direitoEconomia.Id, 1, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
            new(introDireito.Id, 1, 7, 73),
            new(direitoFinanceiro.Id, 1, 7, 73),
        ]);

        var ofertaAds = await client.CreateCourseOffering(campus.Id, ads.Id, gradeAds.Id, period.Id, Shift.Noturno);
        var ofertaDireito = await client.CreateCourseOffering(campus.Id, direito.Id, gradeDireito.Id, period.Id, Shift.Noturno);

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        var turmaMatematica = await client.CreateClass(matematica.Id, chico.Id, period.Id, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        var turmaBancoDeDados = await client.CreateClass(bancoDeDados.Id, chico.Id, period.Id, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        var turmaEstruturaDeDados = await client.CreateClass(estruturaDeDados.Id, chico.Id, period.Id, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        var turmaInfoSociedade = await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);
        var turmaDireitoEconomia = await client.CreateClass(direitoEconomia.Id, ana.Id, period.Id, [new(Day.Terca, Hour.H07_00, Hour.H08_00)]);
        var turmaIntroDireito = await client.CreateClass(introDireito.Id, ana.Id, period.Id, [new(Day.Quarta, Hour.H07_00, Hour.H08_00)]);
        var turmaDireitoFinanceiro = await client.CreateClass(direitoFinanceiro.Id, ana.Id, period.Id, [new(Day.Quinta, Hour.H07_00, Hour.H08_00)]);

        var zaqueu = await client.CreateStudent(ofertaAds.Id, "Zaqueu");
        var maju = await client.CreateStudent(ofertaDireito.Id, "Maju");

        var clientAluno = await _factory.LoggedAsStudent(zaqueu.Email);

        // Act
        var classes = await clientAluno.GetAsync<List<EnrollmentClassOut>>("/matriculas/aluno/turmas");

        // Assert
        classes.Should().HaveCount(4);
        classes.Should().Contain(t => t.Id == turmaMatematica.Id);
        classes.Should().Contain(t => t.Id == turmaBancoDeDados.Id);
        classes.Should().Contain(t => t.Id == turmaEstruturaDeDados.Id);
        classes.Should().Contain(t => t.Id == turmaInfoSociedade.Id);
    }
}
