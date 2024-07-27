namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_institution_teachers()
    {
        // Arrange
        var clientNovaRoma = await _back.LoggedAsAcademic();
        var clientUfpe = await _back.LoggedAsAcademic();

        await clientNovaRoma.CreateTeacher("Chico");
        await clientUfpe.CreateTeacher("Ana");

        // Act
        var teachers = await clientNovaRoma.GetTeachers();

        // Assert
        teachers.Should().HaveCount(1);
        teachers[0].Name.Should().Be("Chico");
    }
}
