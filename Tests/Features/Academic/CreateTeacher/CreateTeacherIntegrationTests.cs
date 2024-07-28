namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var teacher = await client.CreateTeacher("Chico");

        // Assert
        teacher.Id.Should().NotBeEmpty();
        teacher.Name.Should().Be("Chico");
    }

    [Test]
    public async Task Should_not_create_teacher_with_invalid_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var email = TestData.InvalidEmailsList.OrderBy(_ => Guid.NewGuid()).First();

        // Act
        var (_, response) = await client.CreateTeacherTuple("Chico", email);

        // Assert
        await response.AssertBadRequest(new InvalidEmail());
    }

    [Test]
    public async Task Should_not_create_teacher_with_duplicated_email()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var email = TestData.Email;

        var (_, firstResponse) = await client.CreateTeacherTuple("Chico", email);
        var (_, secondResponse) = await client.CreateTeacherTuple("Chico", email);

        // Assert
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        await secondResponse.AssertBadRequest(new EmailAlreadyUsed());
    }
}
