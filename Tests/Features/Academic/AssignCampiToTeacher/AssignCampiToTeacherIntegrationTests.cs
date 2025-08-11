namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_assign_campi_to_teacher()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        TeacherOut teacher = await client.CreateTeacher();

        // Act
        var response = await client.AssignCampiToTeacher(
            teacher.Id,
            [data.Campus.Id]
        );

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_assign_campi_to_teacher_when_teacher_is_missing()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.AssignCampiToTeacher(Guid.CreateVersion7(), []);

        // Assert
        response.ShouldBeError(new TeacherNotFound());
    }

    [Test]
    public async Task Should_not_assign_campi_to_teacher_when_campi_are_invalid()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        TeacherOut teacher = await client.CreateTeacher();

        // Act
        var response = await client.AssignCampiToTeacher(
            teacher.Id,
            [data.Campus.Id, Guid.CreateVersion7()]
        );

        // Assert
        response.ShouldBeError(new InvalidCampusList());
    }
}
