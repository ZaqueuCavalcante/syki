namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_student_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
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

        var classMatematica = await client.CreateClass(matematica.Id, chico.Id, period.Id, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        var classBancoDeDados = await client.CreateClass(bancoDeDados.Id, chico.Id, period.Id, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        var classEstruturaDeDados = await client.CreateClass(estruturaDeDados.Id, chico.Id, period.Id, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);
        var classInfoSociedade = await client.CreateClass(infoSociedade.Id, ana.Id, period.Id, [new(Day.Segunda, Hour.H07_00, Hour.H08_00)]);

        var zaqueu = await client.CreateStudent(courseOfferingAds.Id, "Zaqueu");

        var studentClient = await _back.LoggedAsStudent(zaqueu.Email);

        // Act
        await studentClient.CreateStudentEnrollment([classMatematica.Id, classBancoDeDados.Id]);

        // Assert
        var classes = await studentClient.GetStudentEnrollmentClasses();

        classes.Should().HaveCount(4);
        classes.First(x => x.Id == classMatematica.Id).IsSelected.Should().BeTrue();
        classes.First(x => x.Id == classBancoDeDados.Id).IsSelected.Should().BeTrue();
        classes.First(x => x.Id == classEstruturaDeDados.Id).IsSelected.Should().BeFalse();
        classes.First(x => x.Id == classInfoSociedade.Id).IsSelected.Should().BeFalse();
    }
}
