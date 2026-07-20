namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacher_Should_not_get_teacher_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacher(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacher_Should_not_get_teacher_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacher(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacher_Should_not_get_teacher_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacher(999999);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacher_Should_get_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        var teacher = await client.CreateTeacher("Ana Lima", email).Success();

        // Act
        var result = await client.GetTeacher(teacher.Id);

        // Assert
        var found = result.Success;
        found.Id.Should().Be(teacher.Id);
        found.Name.Should().Be("Ana Lima");
        found.Email.Should().Be(email);
        found.Campi.Should().BeEmpty();
        found.Disciplines.Should().BeEmpty();
    }

    #endregion
}
