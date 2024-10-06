namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_teacher_agenda()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();
        await academicClient.AddStartedAdsClasses(data, _back);

        var teacherClient = await _back.LoggedAsTeacher(data.Teacher.Email);

        // Assert
        var agenda = await teacherClient.GetTeacherAgenda();

        agenda.Should().HaveCount(6);
        agenda.Should().Contain(x => x.Day == Day.Monday);
        agenda.Should().Contain(x => x.Day == Day.Tuesday);
        agenda.Should().Contain(x => x.Day == Day.Wednesday);
        agenda.Should().Contain(x => x.Day == Day.Thursday);
        agenda.Should().Contain(x => x.Day == Day.Friday);
        agenda.Should().Contain(x => x.Day == Day.Saturday);
    }
}
