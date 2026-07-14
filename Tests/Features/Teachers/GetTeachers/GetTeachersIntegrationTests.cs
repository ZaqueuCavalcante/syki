namespace Estud.Tests.Integration;

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
        teachers.Page.Should().Be(1);
        teachers.PageSize.Should().Be(10);
        teachers.Items.First().Name.Should().Be("Ana Lima");
        teachers.Items.Last().Name.Should().Be("Carlos Souza");
    }

    [Test]
    public async Task Teachers_GetTeachers_Should_get_only_the_first_10_teachers_by_default()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        for (var i = 1; i <= 12; i++)
            await client.CreateTeacher($"Professor {i:00}", DataGen.Email);

        // Act
        var result = await client.GetTeachers();

        // Assert
        var teachers = result.Success;
        teachers.Total.Should().Be(12);
        teachers.Page.Should().Be(1);
        teachers.PageSize.Should().Be(10);
        teachers.Items.Should().HaveCount(10);
        teachers.Items.First().Name.Should().Be("Professor 01");
        teachers.Items.Last().Name.Should().Be("Professor 10");
    }

    [Test]
    public async Task Teachers_GetTeachers_Should_get_teachers_from_the_second_page()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        for (var i = 1; i <= 12; i++)
            await client.CreateTeacher($"Professor {i:00}", DataGen.Email);

        // Act
        var result = await client.GetTeachers(page: 2);

        // Assert
        var teachers = result.Success;
        teachers.Total.Should().Be(12);
        teachers.Page.Should().Be(2);
        teachers.Items.Should().HaveCount(2);
        teachers.Items.First().Name.Should().Be("Professor 11");
        teachers.Items.Last().Name.Should().Be("Professor 12");
    }

    [Test]
    public async Task Teachers_GetTeachers_Should_get_teachers_filtered_by_text()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateTeacher("Carlos Souza", DataGen.Email);
        await client.CreateTeacher("Ana Lima", DataGen.Email);

        // Act
        var result = await client.GetTeachers("ana");

        // Assert
        var teachers = result.Success;
        teachers.Total.Should().Be(1);
        teachers.Items.Single().Name.Should().Be("Ana Lima");
    }

    #endregion
}
