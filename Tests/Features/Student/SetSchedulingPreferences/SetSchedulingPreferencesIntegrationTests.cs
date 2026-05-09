namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    // [Test]
    public async Task Should_set_scheduling_preferences()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);
        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        // Act
        var response = await teacherClient.SetSchedulingPreferences(schedules);

        // Assert
        response.ShouldBeSuccess();
    }
}
