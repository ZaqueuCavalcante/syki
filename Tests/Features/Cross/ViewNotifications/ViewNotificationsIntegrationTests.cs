namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_view_user_notifications()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        await client.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        await studentClient.Http.ViewNotifications();

        // Assert
        var response = await studentClient.Http.GetUserNotifications();
        response.Count.Should().Be(1);
        response[0].ViewedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}
