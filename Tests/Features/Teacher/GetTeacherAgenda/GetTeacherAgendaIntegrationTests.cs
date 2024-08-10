namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_teacher_agenda()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod;

        TeacherOut chico = await client.CreateTeacher("Chico");
        TeacherOut ana = await client.CreateTeacher("Ana");

        await client.CreateClass(data.Disciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(data.Disciplines.IntroToWebDev.Id, chico.Id, period.Id, 40, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(data.Disciplines.IntroToComputerNetworks.Id, ana.Id, period.Id, 40, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Assert
        var agenda = await teacherClient.GetTeacherAgenda();

        agenda.Should().HaveCount(2);
        agenda.Should().Contain(x => x.Day == Day.Monday);
        agenda.Should().Contain(x => x.Day == Day.Tuesday);
    }
}
