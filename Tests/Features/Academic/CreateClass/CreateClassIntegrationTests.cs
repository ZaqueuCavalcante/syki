namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_class()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        // Act
        ClassOut @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        @class.Id.Should().NotBeEmpty();
        @class.Discipline.Should().Be(discipline.Name);
        @class.Teacher.Should().Be(teacher.Name);
        @class.Period.Should().Be(period.Id);
        @class.Vacancies.Should().Be(40);
        @class.Status.Should().Be(ClassStatus.OnPreEnrollment);
        @class.Schedules.Should().ContainSingle();
    }

    [Test]
    public async Task Should_not_create_class_without_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateClass(Guid.NewGuid(), Guid.NewGuid(), "2024.1", 40, []);

        // Assert
        response.ShouldBeError(new DisciplineNotFound());
    }

    [Test]
    public async Task Should_not_create_class_without_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();

        // Act
        var response = await client.CreateClass(discipline.Id, Guid.NewGuid(), "2024.1", 40, []);

        // Assert
        response.ShouldBeError(new TeacherNotFound());
    }

    [Test]
    public async Task Should_not_create_class_without_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        
        // Act
        var response = await client.CreateClass(discipline.Id, teacher.Id, "2024.1", 40, []);

        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }

    [Test]
    public async Task Should_not_create_class_with_one_invalid_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H07_00) };

        // Act
        var response = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        response.ShouldBeError(new InvalidSchedule());
    }

    [Test]
    public async Task Should_not_create_class_with_two_invalid_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>()
        {
            new(Day.Monday, Hour.H07_00, Hour.H08_00),
            new(Day.Tuesday, Hour.H09_00, Hour.H08_00),
            new(Day.Wednesday, Hour.H12_00, Hour.H12_00),
        };

        // Act
        var response = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        response.ShouldBeError(new InvalidSchedule());
    }

    [Test]
    public async Task Should_not_create_class_with_conflicting_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        var schedules = new List<ScheduleIn>()
        {
            new(Day.Monday, Hour.H07_00, Hour.H08_00),
            new(Day.Monday, Hour.H07_30, Hour.H08_30),
        };

        // Act
        var response = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        response.ShouldBeError(new ConflictingSchedules());
    }

    [Test]
    public async Task Should_create_class_lessons_for_one_day_one_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();

        var (start, end) = (new DateOnly(2024, 08, 12), new DateOnly(2024, 08, 30));
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1", start, end);

        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };

        // Act
        ClassOut @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        GetAcademicClassOut classDb = await client.GetAcademicClass(@class.Id);
        @classDb.Workload.Should().Be("9h");
        @classDb.Lessons.Should().HaveCount(3);
        @classDb.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), Schedule = "Terça 19:00-22:00" });
        @classDb.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), Schedule = "Terça 19:00-22:00" });
        @classDb.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 27), Schedule = "Terça 19:00-22:00" });
    }

    [Test]
    public async Task Should_create_class_lessons_for_one_day_two_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();

        var (start, end) = (new DateOnly(2024, 08, 12), new DateOnly(2024, 08, 30));
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1", start, end);

        var schedules = new List<ScheduleIn>()
        {
            new(Day.Tuesday, Hour.H19_00, Hour.H22_00),
            new(Day.Tuesday, Hour.H08_15, Hour.H10_30),
        };

        // Act
        ClassOut @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Assert
        GetAcademicClassOut classDb = await client.GetAcademicClass(@class.Id);
        @classDb.Workload.Should().Be("15h e 45min");
        @classDb.Lessons.Should().HaveCount(6);
        @classDb.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), Schedule = "Terça 08:15-10:30" });
        @classDb.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), Schedule = "Terça 19:00-22:00" });
        @classDb.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), Schedule = "Terça 08:15-10:30" });
        @classDb.Lessons[3].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), Schedule = "Terça 19:00-22:00" });
        @classDb.Lessons[4].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 27), Schedule = "Terça 08:15-10:30" });
        @classDb.Lessons[5].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 27), Schedule = "Terça 19:00-22:00" });
    }
}
