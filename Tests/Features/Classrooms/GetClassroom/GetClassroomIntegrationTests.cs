namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_classroom_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetClassroom(id: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_classroom_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetClassroom(id: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_classroom_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetClassroom(id: 99999);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_other_institution_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCampus = (await otherClient.CreateCampus()).Success;
        var otherClassroom = (await otherClient.CreateClassroom(otherCampus.Id)).Success;

        // Act
        var result = await client.GetClassroom(otherClassroom.Id);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classrooms_GetClassroom_Should_get_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus(name: "Campus Agreste")).Success;
        var classroom = (await client.CreateClassroom(campus.Id, name: "Sala 05", capacity: 40)).Success;

        // Act
        var result = await client.GetClassroom(classroom.Id);

        // Assert
        var found = result.Success;
        found.Id.Should().Be(classroom.Id);
        found.Name.Should().Be("Sala 05");
        found.CampusId.Should().Be(campus.Id);
        found.Campus.Should().Be("Campus Agreste");
        found.Capacity.Should().Be(40);
        found.Schedules.Should().BeEmpty();
    }

    #endregion
}
