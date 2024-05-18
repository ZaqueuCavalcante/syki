namespace Syki.Tests.Integration;

[Parallelizable(ParallelScope.All)]
public partial class IntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Deve_criar_um_aluno_o_vinculando_com_uma_oferta()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var grade = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var oferta = await client.CreateCourseOffering(campus.Id, course.Id, grade.Id, period.Id, Shift.Noturno);

        // Act
        var response = await client.CreateStudent(oferta.Id);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.CourseOfferingId.Should().Be(oferta.Id); 
        response.Name.Should().Be("Zezin"); 
    }

    [Test, Ignore("")]
    public async Task Nao_deve_criar_um_aluno_sem_vinculo_com_oferta()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.CreateStudentHttp(Guid.NewGuid());

        // Assert
        await response.AssertBadRequest(Throw.DE012);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_as_disciplines_cursadas_pelo_aluno()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");

        var discipline01 = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var discipline02 = await client.CreateDiscipline("Estrutura de Dados", [course.Id]);
        var discipline03 = await client.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);

        var disciplines = new List<CreateCourseCurriculumDisciplineIn>() { new() { Id = discipline01.Id }, new() { Id = discipline02.Id }, new() { Id = discipline03.Id } };

        var grade = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, disciplines);
        var oferta = await client.CreateCourseOffering(campus.Id, course.Id, grade.Id, period.Id, Shift.Noturno);

        var aluno = await client.CreateStudent(oferta.Id, "Zaqueu");

        var token = await _factory.GetResetPasswordToken(aluno.Email);
        var password = await client.ResetPassword(token!);
        await client.Login(aluno.Email, password);

        // Act
        var response = await client.GetStudentDisciplines();

        // Assert
        response.Count.Should().Be(3);
        response[0].Name.Should().Be("Banco de Dados");
        response[1].Name.Should().Be("Estrutura de Dados");
        response[2].Name.Should().Be("Programação Orientada a Objetos");
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_os_alunos()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var grade = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var oferta = await client.CreateCourseOffering(campus.Id, course.Id, grade.Id, period.Id, Shift.Noturno);

        await client.CreateStudent(oferta.Id, "Zaqueu");
        await client.CreateStudent(oferta.Id, "Maju");

        // Act
        var response = await client.GetAsync<List<StudentOut>>("/alunos");

        // Assert
        response.Count.Should().Be(2); 
    }
}
