namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_students()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        await client.CreateStudent(data.CourseOffering.Id, "Zaqueu");
        await client.CreateStudent(data.CourseOffering.Id, "Maju");

        // Act
        var response = await client.GetStudents();

        // Assert
        response.Count.Should().Be(2); 
    }
}
