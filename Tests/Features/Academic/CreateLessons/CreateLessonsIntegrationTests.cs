namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_class_lessons_for_one_day_one_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        var start = new DateOnly(2024, 08, 12);
        var end = new DateOnly(2024, 08, 30);
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1", start, end);
        var schedules = new List<ScheduleIn>() { new(Day.Tuesday, Hour.H19_00, Hour.H22_00) };
        ClassOut @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Act
        var response = await client.CreateClassLessons(@class.Id);

        // Assert
        response.ShouldBeSuccess();
        GetAcademicClassOut classDb = await client.GetAcademicClass(@class.Id);
        @classDb.Workload.Should().Be("9h");
        @classDb.Lessons.Should().HaveCount(3);
        @classDb.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), Schedule = "19:00-22:00" });
        @classDb.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), Schedule = "19:00-22:00" });
        @classDb.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 27), Schedule = "19:00-22:00" });
    }

    [Test]
    public async Task Should_create_class_lessons_for_one_day_two_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var discipline = await client.CreateDiscipline();
        TeacherOut teacher = await client.CreateTeacher();
        var start = new DateOnly(2024, 08, 12);
        var end = new DateOnly(2024, 08, 30);
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1", start, end);
        var schedules = new List<ScheduleIn>()
        {
            new(Day.Tuesday, Hour.H19_00, Hour.H22_00),
            new(Day.Tuesday, Hour.H08_15, Hour.H10_30),
        };
        ClassOut @class = await client.CreateClass(discipline.Id, teacher.Id, period.Id, 40, schedules);

        // Act
        var response = await client.CreateClassLessons(@class.Id);

        // Assert
        response.ShouldBeSuccess();
        GetAcademicClassOut classDb = await client.GetAcademicClass(@class.Id);
        @classDb.Workload.Should().Be("15h e 45min");
        @classDb.Lessons.Should().HaveCount(6);
        @classDb.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), Schedule = "08:15-10:30" });
        @classDb.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), Schedule = "19:00-22:00" });
        @classDb.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), Schedule = "08:15-10:30" });
        @classDb.Lessons[3].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), Schedule = "19:00-22:00" });
        @classDb.Lessons[4].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 27), Schedule = "08:15-10:30" });
        @classDb.Lessons[5].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 27), Schedule = "19:00-22:00" });
    }

    [Test]
    public async Task Should_not_create_class_lessons_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateClassLessons(Guid.NewGuid());

        // Assert
        response.ShouldBeError(new ClassNotFound());
    }
}
