namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_insights()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod;

        TeacherOut ana = await client.CreateTeacher("Ana");

        await client.CreateClass(data.Disciplines.IntroToComputerNetworks.Id, ana.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _back.LoggedAsTeacher(ana.Email);

        // Act
        var insights = await teacherClient.GetTeacherInsights();

        // Assert
        insights.Classes.Should().Be(1);
    }
}
