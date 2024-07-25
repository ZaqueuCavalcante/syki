namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_student_exam_grades()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        await client.CreateEnrollmentPeriod(period.Id);

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var ads = await client.CreateCourse("ADS");

        var geometria = await client.CreateDiscipline("Geometria Analítica", [ads.Id]);
        var bancoDeDados = await client.CreateDiscipline("Banco de Dados", [ads.Id]);
        var estruturaDeDados = await client.CreateDiscipline("Estrutura de Dados", [ads.Id]);

        var courseCurriculumAds = await client.CreateCourseCurriculum("Grade ADS 1.0", ads.Id,
        [
            new(geometria.Id, 1, 7, 73),
            new(bancoDeDados.Id, 1, 7, 73),
            new(estruturaDeDados.Id, 2, 7, 73),
        ]);

        var courseOfferingAds = await client.CreateCourseOffering(campus.Id, ads.Id, courseCurriculumAds.Id, period.Id, Shift.Noturno);

        var chico = await client.CreateTeacher("Chico");

        var classMatematica = await client.CreateClass(geometria.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        var classBancoDeDados = await client.CreateClass(bancoDeDados.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        var classEstruturaDeDados = await client.CreateClass(estruturaDeDados.Id, chico.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);

        var zaqueu = await client.CreateStudent(courseOfferingAds.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(zaqueu.Email);
        await studentClient.CreateStudentEnrollment([classMatematica.Id, classBancoDeDados.Id, classEstruturaDeDados.Id]);

        // Act
        var response = await studentClient.GetStudentExamGrades();

        // Assert
        response.Count.Should().Be(3);
        response[0].Period.Should().Be(1);
        response[0].Discipline.Should().Be(bancoDeDados.Name);
        response[0].ExamGrades.Should().HaveCount(3);

        response[1].Period.Should().Be(1);
        response[1].Discipline.Should().Be(geometria.Name);
        response[1].ExamGrades.Should().HaveCount(3);

        response[2].Period.Should().Be(2);
        response[2].Discipline.Should().Be(estruturaDeDados.Name);
        response[2].ExamGrades.Should().HaveCount(3);
    }
}
