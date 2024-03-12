using Syki.Shared;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var body = new OfertaIn { };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE010);      
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_nao_existe()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var body = new OfertaIn { CampusId = Guid.NewGuid() };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);
        
        // Assert
        await response.AssertBadRequest(Throw.DE010);     
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_pertence_a_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userUfpe);
        var campusUfpe = await client.CreateCampus("Suassuna", "Recife - PE");

        await client.Login(userNovaRoma);
        await client.CreateCampus("Agreste I", "Caruaru - PE");

        var body = new OfertaIn { CampusId = campusUfpe.Id };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE010);      
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var body = new OfertaIn { CampusId = campus.Id };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);
        
        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_que_nao_existe()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var body = new OfertaIn { CampusId = campus.Id, Periodo = "2025.1" };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);
        
        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userUfpe);
        var periodoUfpe = await client.CreateAcademicPeriod("2023.1");

        await client.Login(userNovaRoma);
        var campusNovaRoma = await client.CreateCampus("Agreste I", "Caruaru - PE");
        await client.CreateAcademicPeriod("2023.2");

        var body = new OfertaIn { CampusId = campusNovaRoma.Id, Periodo = periodoUfpe.Id };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_que_nao_existe()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = Guid.NewGuid() };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_de_outra_faculdade()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userUfpe);
        var curso = await client.NewCurso("Direito");

        await client.Login(userNovaRoma);
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_sem_grade_vinculada()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.NewCurso("Direito");

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_com_grade_que_nao_existe()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.NewCurso("Direito");

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = Guid.NewGuid(), };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    // [Test]
    public async Task Nao_deve_criar_uma_oferta_com_curso_com_grade_que_nao_eh_do_curso_escolhido()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var cursoAds = await client.NewCurso("ADS");
        var cursoDireito = await client.NewCurso("Direito");
        var grade = await client.NewGrade("Grade de ADS 1.0", cursoAds.Id);

        var body = new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = cursoDireito.Id, GradeId = grade.Id, };

        // Act
        var response = await client.PostHttpAsync("/ofertas", body);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    // [Test]
    public async Task Deve_criar_uma_oferta()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.NewCurso("ADS");
        var grade = await client.NewGrade("Grade de ADS 1.0", curso.Id);

        // Act
        var oferta = await client.NewOferta(campus.Id, curso.Id, grade.Id, periodo.Id);

        // Assert
        oferta.Id.Should().NotBeEmpty();
        oferta.GradeId.Should().Be(grade.Id);
        oferta.Periodo.Should().Be(periodo.Id);
    }

    // [Test]
    public async Task Deve_retornar_todas_as_ofertas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.NewCurso("ADS");
        var grade = await client.NewGrade("Grade de ADS 1.0", curso.Id);
        await client.NewOferta(campus.Id, curso.Id, grade.Id, periodo.Id);

        // Act
        var ofertas = await client.GetAsync<List<OfertaOut>>("/ofertas");

        // Assert
        ofertas.Count.Should().Be(1);
    }
}
