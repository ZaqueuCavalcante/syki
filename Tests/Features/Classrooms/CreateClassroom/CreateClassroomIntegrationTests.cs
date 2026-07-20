namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classrooms_CreateClassroom_Should_not_create_classroom_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateClassroom(campusId: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classrooms_CreateClassroom_Should_not_create_classroom_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateClassroom(campusId: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classrooms_CreateClassroom_Should_not_create_classroom_when_campus_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateClassroom(campusId: 999999);

        // Assert
        result.ShouldBeError(CampusNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classrooms_CreateClassroom_Should_create_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();

        // Act
        var result = await client.CreateClassroom(campus.Id, name: "Sala 05", capacity: 40);

        // Assert
        var classroom = result.Success;
        classroom.Id.Should().NotBe(0);
    }

    #endregion
}
