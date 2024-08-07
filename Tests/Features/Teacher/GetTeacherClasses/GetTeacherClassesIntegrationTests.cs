namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_teacher_classes()
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

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().HaveCount(2);
        classes[0].Discipline.Should().Be(data.Disciplines.IntroToWebDev.Name);
        classes[1].Discipline.Should().Be(data.Disciplines.DiscreteMath.Name);
    }

    [Test]
    public async Task Should_not_return_other_teacher_classes()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod;

        var chico = await client.CreateTeacher("Chico");
        var ana = await client.CreateTeacher("Ana");

        await client.CreateClass(data.Disciplines.IntroToComputerNetworks.Id, ana.Id, period.Id, 40, [new(Day.Quarta, Hour.H07_00, Hour.H10_00)]);

        var teacherClient = await _back.LoggedAsTeacher(chico.Email);

        // Act
        var classes = await teacherClient.GetTeacherClasses();

        // Assert
        classes.Should().BeEmpty();
    }
}
