namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_students()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        await client.CreateStudent(data.AdsCourseOffering.Id, "Maju");

        // Act
        var response = await client.GetStudents();

        // Assert
        response.Count.Should().Be(2); 
    }
}
