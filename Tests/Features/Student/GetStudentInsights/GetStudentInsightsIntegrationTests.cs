namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_student_insights()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");

        var bd = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var ed = await client.CreateDiscipline("Estrutura de Dados", [course.Id]);
        var poo = await client.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(bd.Id, 1, 10, 70), new(ed.Id, 2, 8, 55), new(poo.Id, 3, 12, 60) };
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, disciplines);

        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var insights = await studentClient.GetStudentInsights();

        // Assert
        insights.TotalDisciplines.Should().Be(3);
    }
}
