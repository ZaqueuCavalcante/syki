namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_UpdateTeacher_Should_not_update_teacher_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateTeacher(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_UpdateTeacher_Should_not_update_teacher_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateTeacher(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Teachers_UpdateTeacher_Should_not_update_teacher_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateTeacher(1, name: name);

        // Assert
        result.ShouldBeError(InvalidTeacherName.I);
    }

    [Test]
    [TestCase("")]
    [TestCase("not-an-email")]
    public async Task Teachers_UpdateTeacher_Should_not_update_teacher_with_invalid_email(string email)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateTeacher(1, email: email);

        // Assert
        result.ShouldBeError(InvalidEmail.I);
    }

    [Test]
    public async Task Teachers_UpdateTeacher_Should_not_update_teacher_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateTeacher(999999);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    [Test]
    public async Task Teachers_UpdateTeacher_Should_not_update_teacher_when_email_already_used()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var usedEmail = DataGen.Email;
        await client.CreateTeacher("Carlos Souza", usedEmail);
        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();

        // Act
        var result = await client.UpdateTeacher(teacher.Id, name: "Ana Lima", email: usedEmail);

        // Assert
        result.ShouldBeError(EmailAlreadyUsed.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_UpdateTeacher_Should_update_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        var newEmail = DataGen.Email;

        // Act
        var result = await client.UpdateTeacher(teacher.Id, name: "Ana Paula Lima", email: newEmail);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetTeacher(teacher.Id).Success();
        updated.Name.Should().Be("Ana Paula Lima");
        updated.Email.Should().Be(newEmail);
    }

    #endregion
}
