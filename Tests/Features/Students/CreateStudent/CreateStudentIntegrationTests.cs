namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_CreateStudent_Should_not_create_student_when_email_already_used()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        await client.CreateStudent(DataGen.UserName, email);

        // Act
        var result = await client.CreateStudent(DataGen.UserName, email);

        // Assert
        result.ShouldBeError(EmailAlreadyUsed.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_CreateStudent_Should_create_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeSuccess();
        var student = result.Success;
        student.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
