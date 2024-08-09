namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_student_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        CourseOut course = await client.CreateCourse("ADS");

        var discipline01 = await client.CreateDiscipline("Banco de Dados", [course.Id]);
        var discipline02 = await client.CreateDiscipline("Estrutura de Dados", [course.Id]);
        var discipline03 = await client.CreateDiscipline("Programação Orientada a Objetos", [course.Id]);
        var disciplines = new List<CreateCourseCurriculumDisciplineIn>() { new() { Id = discipline01.Id }, new() { Id = discipline02.Id }, new() { Id = discipline03.Id } };

        CourseCurriculumOut courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id, disciplines);
        CourseOfferingOut courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        StudentOut student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var response = await studentClient.GetStudentDisciplines();

        // Assert
        response.Count.Should().Be(3);
        response[0].Name.Should().Be("Banco de Dados");
        response[1].Name.Should().Be("Estrutura de Dados");
        response[2].Name.Should().Be("Programação Orientada a Objetos");
    }
}
