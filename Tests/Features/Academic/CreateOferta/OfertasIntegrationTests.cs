namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.CreateOfertaHttp(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE010);      
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_quando_o_campus_pertence_a_outra_institution()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");
        var campusUfpe = await clientUfpe.CreateCampus("Suassuna", "Recife - PE");

        // Act
        var response = await clientNovaRoma.CreateOfertaHttp(campusUfpe.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE010);      
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_curso()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var response = await client.CreateOfertaHttp(campus.Id, Guid.NewGuid(), Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_com_curso_de_outra_institution()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var cursoUfpe = await clientUfpe.CreateCourse("Direito");
        var campusNovaRoma = await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var response = await clientNovaRoma.CreateOfertaHttp(campusNovaRoma.Id, cursoUfpe.Id, Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE002);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_sem_grade_vinculada()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var curso = await client.CreateCourse("Direito");

        // Act
        var response = await client.CreateOfertaHttp(campus.Id, curso.Id, Guid.NewGuid(), "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_com_grade_que_nao_eh_do_curso_escolhido()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var cursoAds = await client.CreateCourse("ADS");
        var cursoDireito = await client.CreateCourse("Direito");
        var grade = await client.CreateGrade("Grade de ADS 1.0", cursoAds.Id);

        // Act
        var response = await client.CreateOfertaHttp(campus.Id, cursoDireito.Id, grade.Id, "2024.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE011);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_sem_vinculo_com_periodo()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var curso = await client.CreateCourse("Direito");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);

        // Act
        var response = await client.CreateOfertaHttp(campus.Id, curso.Id, grade.Id, "2024.1", Shift.Matutino);
        
        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_uma_oferta_com_periodo_de_outra_institution()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        await clientUfpe.CreateAcademicPeriod("2023.1");

        var campus = await clientNovaRoma.CreateCampus("Agreste I", "Caruaru - PE");
        var curso = await clientNovaRoma.CreateCourse("Direito");
        var grade = await clientNovaRoma.CreateGrade("Grade de ADS 1.0", curso.Id);

        // Act
        var response = await clientNovaRoma.CreateOfertaHttp(campus.Id, curso.Id, grade.Id, "2023.1", Shift.Matutino);

        // Assert
        await response.AssertBadRequest(Throw.DE005);
    }

    [Test, Ignore("")]
    public async Task Deve_criar_uma_oferta()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCourse("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);

        // Act
        var oferta = await client.CreateOferta(campus.Id, curso.Id, grade.Id, period.Id, Shift.Matutino);

        // Assert
        oferta.Id.Should().NotBeEmpty();
        oferta.CourseCurriculumId.Should().Be(grade.Id);
        oferta.Period.Should().Be(period.Id);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_todas_as_ofertas()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.CreateCourse("ADS");
        var grade = await client.CreateGrade("Grade de ADS 1.0", curso.Id);
        await client.CreateOferta(campus.Id, curso.Id, grade.Id, period.Id, Shift.Noturno);

        // Act
        var ofertas = await client.GetAsync<List<CourseOfferingOut>>("/ofertas");

        // Assert
        ofertas.Count.Should().Be(1);
    }
}
