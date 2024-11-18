namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_insights()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _api);

        var teacherClient = await _api.LoggedAsTeacher(data.Teacher.Email);

        var lessons = data.AdsClasses.HumanMachineInteractionDesign.Lessons;
        await teacherClient.CreateLessonAttendance(lessons[0].Id, [data.Student.Id]);

        // Act
        var insights = await teacherClient.GetTeacherInsights();

        // Assert
        insights.Classes.Should().Be(6);
        insights.Students.Should().Be(1);
        insights.FinalizedLessons.Should().Be(1);
        insights.TotalLessons.Should().Be(129);
    }
}
