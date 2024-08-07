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

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        await client.CreateClass(data.Disciplines.DiscreteMath.Id, chico.Id, period.Id, 40, [new(Day.Segunda, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(data.Disciplines.IntroToWebDev.Id, chico.Id, period.Id, 40, [new(Day.Terca, Hour.H07_00, Hour.H10_00)]);
        await client.CreateClass(data.Disciplines.IntroToComputerNetworks.Id, ana.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Assert
        var agenda = await teacherClient.GetTeacherAgenda();

        agenda.Should().HaveCount(2);
        agenda.Should().Contain(x => x.Day == Day.Segunda);
        agenda.Should().Contain(x => x.Day == Day.Terca);
    }
}
