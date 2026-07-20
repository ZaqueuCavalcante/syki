namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classrooms_GetClassrooms_Should_not_get_classrooms_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetClassrooms();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classrooms_GetClassrooms_Should_not_get_classrooms_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetClassrooms();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classrooms_GetClassrooms_Should_get_the_institution_classrooms()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus(name: "Agreste I").Success();
        var classroom = await client.CreateClassroom(campus.Id, name: "Sala 05", capacity: 40).Success();

        // Act
        var result = await client.GetClassrooms();

        // Assert
        var classrooms = result.Success;
        classrooms.Should().HaveCount(1);
        var item = classrooms.Single(x => x.Id == classroom.Id);
        item.Name.Should().Be("Sala 05");
        item.Capacity.Should().Be(40);
        item.CampusId.Should().Be(campus.Id);
        item.Campus.Should().Be("Agreste I");
    }

    #endregion
}
