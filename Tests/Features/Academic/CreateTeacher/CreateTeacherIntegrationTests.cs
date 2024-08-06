namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var teacher = await client.CreateTeacher("Richard");

        // Assert
        teacher.Id.Should().NotBeEmpty();
        teacher.Name.Should().Be("Richard");
    }

    [Test]
    public async Task Should_not_create_teacher_with_invalid_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var email = TestData.InvalidEmailsList.OrderBy(_ => Guid.NewGuid()).First();

        // Act
        var response = await client.CreateTeacher2("Chico", email);

        // Assert
        response.ShouldBeError(new InvalidEmail());
    }

    [Test]
    public async Task Should_not_create_teacher_with_duplicated_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var email = TestData.Email;

        var firstResponse = await client.CreateTeacher2("Chico", email);
        var secondResponse = await client.CreateTeacher2("Chico", email);

        // Assert
        firstResponse.ShouldBeSuccess();
        secondResponse.ShouldBeError(new EmailAlreadyUsed());
    }

    [Test]
    public async Task Should_create_teacher_only_with_teacher_role()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var teacher = await client.CreateTeacher();

        // Assert
        using var userManager = _back.GetUserManager();
        var user = await userManager.FindByEmailAsync(teacher.Email);
        var isOnlyInTeacherRole = await userManager.IsOnlyInRole(user!, UserRole.Teacher);
        isOnlyInTeacherRole.Should().BeTrue();
    }
}
