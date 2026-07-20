namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudent_Should_not_get_student_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudent(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudent_Should_not_get_student_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudent(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_GetStudent_Should_not_get_student_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudent(999999);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudent_Should_get_the_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        var student = await client.CreateStudent("Ana Lima", email).Success();

        // Act
        var result = await client.GetStudent(student.Id);

        // Assert
        var found = result.Success;
        found.Id.Should().Be(student.Id);
        found.Name.Should().Be("Ana Lima");
        found.Email.Should().Be(email);
        found.CurrentCourseOfferingId.Should().BeNull();
    }

    #endregion
}
