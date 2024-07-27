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
    public async Task Should_create_many_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateTeacher("Chico");
        await client.CreateTeacher("Ana");

        // Assert
        var teachers = await client.GetTeachers();
        teachers.Should().HaveCount(2);
    }
}
