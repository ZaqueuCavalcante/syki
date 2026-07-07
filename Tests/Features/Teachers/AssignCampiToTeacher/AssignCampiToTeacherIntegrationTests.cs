namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_AssignCampiToTeacher_Should_not_assign_campi_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.AssignCampiToTeacher(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_AssignCampiToTeacher_Should_not_assign_campi_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.AssignCampiToTeacher(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_AssignCampiToTeacher_Should_not_assign_campi_when_teacher_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.AssignCampiToTeacher(999999, []);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    [Test]
    public async Task Teachers_AssignCampiToTeacher_Should_not_assign_campi_when_a_campus_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var teacher = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;

        // Act
        var result = await client.AssignCampiToTeacher(teacher.Id, [999999]);

        // Assert
        result.ShouldBeError(InvalidCampusList.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_AssignCampiToTeacher_Should_assign_campi_to_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var alpha = (await client.CreateCampus(name: "Alpha")).Success;
        var beta = (await client.CreateCampus(name: "Beta")).Success;
        var teacher = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;

        // Act
        var result = await client.AssignCampiToTeacher(teacher.Id, [alpha.Id, beta.Id]);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = (await client.GetTeacher(teacher.Id)).Success;
        updated.Campi.Should().HaveCount(2);
        updated.Campi.Should().Contain(x => x.Id == alpha.Id);
        updated.Campi.Should().Contain(x => x.Id == beta.Id);
    }

    #endregion
}
