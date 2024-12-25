namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_teacher()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        TeacherOut teacher = await client.CreateTeacher("Richard");

        // Assert
        teacher.Id.Should().NotBeEmpty();
        teacher.Name.Should().Be("Richard");
    }

    [Test]
    public async Task Should_not_create_teacher_with_invalid_email()
    {
        // Arrange
        var email = TestData.InvalidEmails().PickRandom().First().ToString()!;
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateTeacher("Chico", email);

        // Assert
        response.ShouldBeError(new InvalidEmail());
    }

    [Test]
    public async Task Should_not_create_teacher_with_duplicated_email()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var email = TestData.Email;

        var firstResponse = await client.CreateTeacher("Chico", email);
        var secondResponse = await client.CreateTeacher("Chico", email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());
    }

    [Test]
    public async Task Should_create_teacher_only_with_teacher_role()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        TeacherOut teacher = await client.CreateTeacher();

        // Assert
        using var userManager = _api.GetUserManager();
        var user = await userManager.FindByEmailAsync(teacher.Email);
        var isOnlyInTeacherRole = await userManager.IsOnlyInRole(user!, UserRole.Teacher);
        isOnlyInTeacherRole.Should().BeTrue();
    }
}
