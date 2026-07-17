namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classrooms_UpdateClassroom_Should_not_update_classroom_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateClassroom(id: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classrooms_UpdateClassroom_Should_not_update_classroom_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateClassroom(id: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Classrooms_UpdateClassroom_Should_not_update_classroom_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var classroom = (await client.CreateClassroom(campus.Id)).Success;

        // Act
        var result = await client.UpdateClassroom(classroom.Id, name, capacity: 50);

        // Assert
        result.ShouldBeError(InvalidClassroomName.I);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task Classrooms_UpdateClassroom_Should_not_update_classroom_with_invalid_capacity(int capacity)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var classroom = (await client.CreateClassroom(campus.Id)).Success;

        // Act
        var result = await client.UpdateClassroom(classroom.Id, name: "Sala 10", capacity: capacity);

        // Assert
        result.ShouldBeError(InvalidClassroomCapacity.I);
    }

    [Test]
    public async Task Classrooms_UpdateClassroom_Should_not_update_classroom_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateClassroom(id: 99999);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    [Test]
    public async Task Classrooms_UpdateClassroom_Should_not_update_other_institution_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCampus = (await otherClient.CreateCampus()).Success;
        var otherClassroom = (await otherClient.CreateClassroom(otherCampus.Id)).Success;

        // Act
        var result = await client.UpdateClassroom(otherClassroom.Id, name: "Sala 10", capacity: 50);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classrooms_UpdateClassroom_Should_update_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var classroom = (await client.CreateClassroom(campus.Id, name: "Sala 05", capacity: 40)).Success;

        // Act
        var result = await client.UpdateClassroom(classroom.Id, name: "Sala 10", capacity: 50);

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(classroom.Id);
        updated.Name.Should().Be("Sala 10");
        updated.CampusId.Should().Be(campus.Id);
        updated.Capacity.Should().Be(50);
    }

    #endregion
}
