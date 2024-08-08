namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_agenda()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await client.CreateEnrollmentPeriod(period.Id);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        CourseOut ads = await client.CreateCourse("ADS");
        CourseOut direito = await client.CreateCourse("Direito");

        var matematica = await client.CreateDiscipline("Matemática Discreta", [ads.Id]);
        var bancoDeDados = await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        var estruturaDeDados = await client.CreateDiscipline("Estrutura de Dados", [ads.Id]);
        var infoSociedade = await client.CreateDiscipline("Informática e Sociedade", [ads.Id, direito.Id]);

        var courseCurriculumAds = await client.CreateCourseCurriculum("Grade ADS 1.0", ads.Id,
        [
            new(matematica.Id, 1, 7, 73),
            new(bancoDeDados.Id, 1, 7, 73),
            new(estruturaDeDados.Id, 2, 7, 73),
            new(infoSociedade.Id, 2, 7, 73),
        ]);

        var courseOfferingAds = await client.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        var classMatematica = await client.CreateClass(matematica.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        var classBancoDeDados = await client.CreateClass(bancoDeDados.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        var classEstruturaDeDados = await client.CreateClass(estruturaDeDados.Id, chico.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);

        var zaqueu = await client.CreateStudent(courseOfferingAds.Id, "Zaqueu");

        var studentClient = await _back.LoggedAsStudent(zaqueu.Email);
        await studentClient.CreateStudentEnrollment([classMatematica.Id, classBancoDeDados.Id, classEstruturaDeDados.Id]);

        // Act
        var agenda = await studentClient.GetStudentAgenda();

        // Assert
        agenda.Should().HaveCount(3);
    }
}
