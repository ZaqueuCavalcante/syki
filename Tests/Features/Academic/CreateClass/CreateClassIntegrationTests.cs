namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_class()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        // Act
        var @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        @class.Id.Should().NotBeEmpty();
        @class.Discipline.Should().Be(discipline.Name);
        @class.Teacher.Should().Be(teacher.Name);
        @class.Period.Should().Be(period.Id);
        @class.Vacancies.Should().Be(40);
        @class.Status.Should().Be(ClassStatus.OnEnrollmentPeriod);
        @class.Schedules.Should().ContainSingle();
    }

    [Test]
    public async Task Should_not_create_class_without_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateClassHttp(Guid.NewGuid(), Guid.NewGuid(), "2024.1", 40, []);

        // Assert
        await response.AssertBadRequest(new DisciplineNotFound());
    }

    [Test]
    public async Task Should_not_create_class_without_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();

        // Act
        var response = await client.CreateClassHttp(discipline.Id, Guid.NewGuid(), "2024.1", 40, []);

        // Assert
        await response.AssertBadRequest(new TeacherNotFound());
    }

    [Test]
    public async Task Should_not_create_class_without_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        
        // Act
        var response = await client.CreateClassHttp(discipline.Id, teacher.Id, "2024.1", 40, []);

        // Assert
        await response.AssertBadRequest(new AcademicPeriodNotFound());
    }

    [Test]
    public async Task Should_not_create_class_with_invalid_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Segunda, Hour.H07_00, Hour.H07_00) };

        // Act
        var response = await client.CreateClassHttp(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        await response.AssertBadRequest(new InvalidSchedule());
    }

    [Test]
    public async Task Should_not_create_class_with_conflicting_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        var teacher = await client.CreateTeacher();
        var period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>()
        {
            new(Day.Segunda, Hour.H07_00, Hour.H08_00),
            new(Day.Segunda, Hour.H07_30, Hour.H08_30),
        };

        // Act
        var response = await client.CreateClassHttp(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        await response.AssertBadRequest(new ConflictingSchedules());
    }
}
