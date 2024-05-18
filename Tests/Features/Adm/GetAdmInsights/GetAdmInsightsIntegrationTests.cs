namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_adm_insights()
    {
        // Arrange
        var academicClient = await _factory.LoggedAsAcademic();

        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");

        var bd = await academicClient.CreateDiscipline("Banco de Dados", [course.Id]);
        var ed = await academicClient.CreateDiscipline("Estrutura de Dados", [course.Id]);
        var poo = await academicClient.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn> { new(bd.Id, 1, 10, 70), new(ed.Id, 2, 8, 55), new(poo.Id, 3, 12, 60) };

        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, disciplines);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Matutino);

        await academicClient.CreateTeacher("Chico");
        await academicClient.CreateTeacher("Ana");

        await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");
        await academicClient.CreateStudent(courseOffering.Id, "Maju");

        var admClient = await _factory.LoggedAsAdm();

        // Act
        var insights = await admClient.GetAdmInsights();

        // Assert
        insights.Institutions.Should().BeGreaterThanOrEqualTo(1);
        insights.Users.Should().BeGreaterThanOrEqualTo(1);
        insights.Campi.Should().BeGreaterThanOrEqualTo(1);
        insights.Courses.Should().BeGreaterThanOrEqualTo(1);
        insights.Disciplines.Should().BeGreaterThanOrEqualTo(3);
        insights.CourseCurriculums.Should().BeGreaterThanOrEqualTo(1);
        insights.CourseOfferings.Should().BeGreaterThanOrEqualTo(1);
        insights.Teachers.Should().BeGreaterThanOrEqualTo(2);
        insights.Students.Should().BeGreaterThanOrEqualTo(2);
    }
}
