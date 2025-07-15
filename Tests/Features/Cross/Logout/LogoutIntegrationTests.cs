using System.Net.Http.Json;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_logout()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.Http.Logout();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = new CreateCourseIn { Name = "Direito", Type = CourseType.Bacharelado, Disciplines = [] };
        var courseResponse = await client.Http.PostAsJsonAsync("/academic/courses", data);
        courseResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
