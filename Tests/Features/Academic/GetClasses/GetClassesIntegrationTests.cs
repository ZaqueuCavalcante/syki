namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_institution_classes()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        var @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, schedules);

        // Act
        var classes = await client.GetClasses();

        // Assert
        classes.Count.Should().Be(1);
        classes[0].Id.Should().Be(@class.Id);
    }
}
