namespace Estud.Tests.Integration;

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
        students.Page.Should().Be(1);
        students.PageSize.Should().Be(10);
        students.Items.First().Name.Should().Be("Ana Lima");
        students.Items.Last().Name.Should().Be("Carlos Souza");
    }

    [Test]
    public async Task Students_GetStudents_Should_get_only_the_first_10_students_by_default()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        for (var i = 1; i <= 12; i++)
            await client.CreateStudent($"Aluno {i:00}", DataGen.Email);

        // Act
        var result = await client.GetStudents();

        // Assert
        var students = result.Success;
        students.Total.Should().Be(12);
        students.Page.Should().Be(1);
        students.PageSize.Should().Be(10);
        students.Items.Should().HaveCount(10);
        students.Items.First().Name.Should().Be("Aluno 01");
        students.Items.Last().Name.Should().Be("Aluno 10");
    }

    [Test]
    public async Task Students_GetStudents_Should_get_students_from_the_second_page()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        for (var i = 1; i <= 12; i++)
            await client.CreateStudent($"Aluno {i:00}", DataGen.Email);

        // Act
        var result = await client.GetStudents(page: 2);

        // Assert
        var students = result.Success;
        students.Total.Should().Be(12);
        students.Page.Should().Be(2);
        students.Items.Should().HaveCount(2);
        students.Items.First().Name.Should().Be("Aluno 11");
        students.Items.Last().Name.Should().Be("Aluno 12");
    }

    [Test]
    public async Task Students_GetStudents_Should_get_students_filtered_by_text()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateStudent("Carlos Souza", DataGen.Email);
        await client.CreateStudent("Ana Lima", DataGen.Email);

        // Act
        var result = await client.GetStudents("ana");

        // Assert
        var students = result.Success;
        students.Total.Should().Be(1);
        students.Items.Single().Name.Should().Be("Ana Lima");
    }

    #endregion
}
