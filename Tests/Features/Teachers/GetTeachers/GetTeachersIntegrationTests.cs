namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeachers_Should_not_get_teachers_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeachers();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeachers_Should_not_get_teachers_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeachers();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeachers_Should_get_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateTeacher("Carlos Souza", DataGen.Email);
        await client.CreateTeacher("Ana Lima", DataGen.Email);

        // Act
        var result = await client.GetTeachers();

        // Assert
        var teachers = result.Success;
        teachers.Total.Should().Be(2);
        teachers.Items.First().Name.Should().Be("Ana Lima");
        teachers.Items.Last().Name.Should().Be("Carlos Souza");
    }

    #endregion
}
