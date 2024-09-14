using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Tests.Features.Academic.CreateLessons;

public class CreateLessonsUnitTests
{
    [Test]
    public void Should_create_lessons_for_one_day_one_schedule_19_00_to_20_00()
    {
        // Arrange
        var @class = GetClass("05/08/2024", "23/08/2024", [new(Day.Monday, Hour.H19_00, Hour.H20_00)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*60);
        @class.Lessons.Should().HaveCount(3);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 05), StartAt = Hour.H19_00, EndAt = Hour.H20_00 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 12), StartAt = Hour.H19_00, EndAt = Hour.H20_00 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 19), StartAt = Hour.H19_00, EndAt = Hour.H20_00 });
    }

    [Test]
    public void Should_create_lessons_for_one_day_one_schedule_09_30_to_12_45()
    {
        // Arrange
        var @class = GetClass("05/08/2024", "23/08/2024", [new(Day.Tuesday, Hour.H09_30, Hour.H12_45)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*(3*60+15));
        @class.Lessons.Should().HaveCount(3);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 06), StartAt = Hour.H09_30, EndAt = Hour.H12_45 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 13), StartAt = Hour.H09_30, EndAt = Hour.H12_45 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 20), StartAt = Hour.H09_30, EndAt = Hour.H12_45 });
    }

    [Test]
    public void Should_create_lessons_for_one_day_one_schedule_20_15_to_22_30()
    {
        // Arrange
        var @class = GetClass("05/08/2024", "23/08/2024", [new(Day.Wednesday, Hour.H20_15, Hour.H22_30)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*(2*60+15));
        @class.Lessons.Should().HaveCount(3);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 07), StartAt = Hour.H20_15, EndAt = Hour.H22_30 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 14), StartAt = Hour.H20_15, EndAt = Hour.H22_30 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 21), StartAt = Hour.H20_15, EndAt = Hour.H22_30 });
    }

    [Test]
    public void Should_create_lessons_for_one_day_two_schedules()
    {
        // Arrange
        var @class = GetClass(
            "27/08/2024", "13/09/2024",
            [new(Day.Thursday, Hour.H15_15, Hour.H18_45),
            new(Day.Thursday, Hour.H09_00, Hour.H10_00)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*(4*60+30));
        @class.Lessons.Should().HaveCount(6);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[3].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[4].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[5].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
    }

    [Test]
    public void Should_create_lessons_for_one_day_three_schedules()
    {
        // Arrange
        var @class = GetClass(
            "27/08/2024", "13/09/2024",
            [new(Day.Thursday, Hour.H08_30, Hour.H12_00),
            new(Day.Thursday, Hour.H13_15, Hour.H18_30),
            new(Day.Thursday, Hour.H20_45, Hour.H23_00)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*11*60);
        @class.Lessons.Should().HaveCount(9);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H08_30, EndAt = Hour.H12_00 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H13_15, EndAt = Hour.H18_30 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H20_45, EndAt = Hour.H23_00 });
        @class.Lessons[3].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H08_30, EndAt = Hour.H12_00 });
        @class.Lessons[4].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H13_15, EndAt = Hour.H18_30 });
        @class.Lessons[5].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H20_45, EndAt = Hour.H23_00 });
        @class.Lessons[6].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H08_30, EndAt = Hour.H12_00 });
        @class.Lessons[7].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H13_15, EndAt = Hour.H18_30 });
        @class.Lessons[8].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H20_45, EndAt = Hour.H23_00 });
    }

    [Test]
    public void Should_create_lessons_for_two_days_one_schedule_per_day()
    {
        // Arrange
        var @class = GetClass(
            "26/08/2024", "13/09/2024",
            [new(Day.Thursday, Hour.H09_00, Hour.H10_00),
            new(Day.Monday, Hour.H15_15, Hour.H18_45)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*(4*60+30));
        @class.Lessons.Should().HaveCount(6);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 26), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 02), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[3].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[4].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 09), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[5].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
    }

    [Test]
    public void Should_create_lessons_for_two_days_one_schedule_on_first_day_and_two_schedules_on_second_day()
    {
        // Arrange
        var @class = GetClass(
            "26/08/2024", "13/09/2024",
            [new(Day.Thursday, Hour.H20_00, Hour.H22_00),
            new(Day.Monday, Hour.H15_15, Hour.H18_45),
            new(Day.Thursday, Hour.H09_00, Hour.H10_00)]);

        // Act
        @class.CreateLessons();

        // Assert
        @class.Workload.Should().Be(3*(6*60+30));
        @class.Lessons.Should().HaveCount(9);
        @class.Lessons[0].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 26), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[1].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[2].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 08, 29), StartAt = Hour.H20_00, EndAt = Hour.H22_00 });
        @class.Lessons[3].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 02), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[4].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[5].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 05), StartAt = Hour.H20_00, EndAt = Hour.H22_00 });
        @class.Lessons[6].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 09), StartAt = Hour.H15_15, EndAt = Hour.H18_45 });
        @class.Lessons[7].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H09_00, EndAt = Hour.H10_00 });
        @class.Lessons[8].Should().BeEquivalentTo(new { Date = new DateOnly(2024, 09, 12), StartAt = Hour.H20_00, EndAt = Hour.H22_00 });
    }

    private Class GetClass(string start, string end, List<Schedule> schedules)
    {
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2024.2";
        const int vacancies = 40;

        var @class = Class.New(institutionId, disciplineId, teacherId, period, vacancies, schedules).GetSuccess();
        @class.Period = AcademicPeriod.New(period, institutionId, start.ToDateOnly(), end.ToDateOnly());

        return @class;
    }
}
