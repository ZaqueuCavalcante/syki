namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_teacher_insights()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };
        await client.CreateClass(discipline.Id, teacher.Id, period.Id, schedules);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);

        // Act
        var insights = await teacherClient.GetTeacherInsights();

        // Assert
        insights.Classes.Should().Be(1);
    }
}
