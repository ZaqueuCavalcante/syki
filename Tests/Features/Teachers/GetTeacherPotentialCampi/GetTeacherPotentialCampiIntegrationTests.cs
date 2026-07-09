namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherPotentialCampi_Should_not_get_potential_campi_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherPotentialCampi(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherPotentialCampi_Should_not_get_potential_campi_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherPotentialCampi(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherPotentialCampi_Should_not_get_potential_campi_when_teacher_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherPotentialCampi(999999);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherPotentialCampi_Should_get_the_campi_not_assigned_to_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var alpha = (await client.CreateCampus(name: "Alpha")).Success;
        var beta = (await client.CreateCampus(name: "Beta")).Success;
        var teacher = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;

        // Act
        var result = await client.GetTeacherPotentialCampi(teacher.Id);

        // Assert
        var items = result.Success.Items;
        items.Should().HaveCount(2);
        items.Should().Contain(x => x.Id == alpha.Id);
        items.Should().Contain(x => x.Id == beta.Id);
    }

    [Test]
    public async Task Teachers_GetTeacherPotentialCampi_Should_not_get_campi_already_assigned_to_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var alpha = (await client.CreateCampus(name: "Alpha")).Success;
        var beta = (await client.CreateCampus(name: "Beta")).Success;
        var teacher = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;
        await client.AssignCampiToTeacher(teacher.Id, [alpha.Id]);

        // Act
        var result = await client.GetTeacherPotentialCampi(teacher.Id);

        // Assert
        var items = result.Success.Items;
        items.Should().ContainSingle(x => x.Id == beta.Id);
        items.Should().NotContain(x => x.Id == alpha.Id);
    }

    [Test]
    public async Task Teachers_GetTeacherPotentialCampi_Should_filter_potential_campi_by_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var alpha = (await client.CreateCampus(name: "Alpha")).Success;
        await client.CreateCampus(name: "Beta");
        var teacher = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;

        // Act
        var result = await client.GetTeacherPotentialCampi(teacher.Id, name: "Alph");

        // Assert
        var items = result.Success.Items;
        items.Should().ContainSingle(x => x.Id == alpha.Id);
    }

    #endregion
}
