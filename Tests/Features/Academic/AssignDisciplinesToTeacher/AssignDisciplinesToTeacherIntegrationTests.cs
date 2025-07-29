namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_assign_disciplines_to_teacher()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        await client.AddStartedAdsClasses(data, _api);

        // Act
        var response = await client.AssignDisciplinesToTeacher(
            data.Teacher.Id,
            [data.AdsDisciplines.DiscreteMath.Id]
        );

        // Assert
        response.ShouldBeSuccess();
    }

    [Test]
    public async Task Should_not_assign_disciplines_to_teacher_when_teacher_is_missing()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.AssignDisciplinesToTeacher(Guid.CreateVersion7(), []);

        // Assert
        response.ShouldBeError(new TeacherNotFound());
    }

    [Test]
    public async Task Should_not_assign_disciplines_to_teacher_when_disciplines_are_invalid()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        await client.AddStartedAdsClasses(data, _api);

        // Act
        var response = await client.AssignDisciplinesToTeacher(
            data.Teacher.Id,
            [data.DireitoDisciplines.GeneralTheoryOfLaw.Id, Guid.CreateVersion7()]
        );

        // Assert
        response.ShouldBeError(new InvalidDisciplinesList());
    }
}
