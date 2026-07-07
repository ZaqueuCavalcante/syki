namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudents_Should_not_get_students_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudents();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudents_Should_not_get_students_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudents();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudents_Should_get_students()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateStudent("Carlos Souza", DataGen.Email);
        await client.CreateStudent("Ana Lima", DataGen.Email);

        // Act
        var result = await client.GetStudents();

        // Assert
        var students = result.Success;
        students.Total.Should().Be(2);
        students.Items.First().Name.Should().Be("Ana Lima");
        students.Items.Last().Name.Should().Be("Carlos Souza");
    }

    #endregion
}
