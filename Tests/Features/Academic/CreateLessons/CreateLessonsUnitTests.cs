using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Tests.Features.Academic.CreateLessons;

public class CreateLessonsUnitTests
{
    [Test]
    public void Should_create_lessons_for_one_day_schedule()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2023.2";
        const int vacancies = 40;
        var schedules = new List<Schedule>() { new(Day.Monday, Hour.H19_00, Hour.H20_00) };
        var @class = new Class(institutionId, disciplineId, teacherId, period, vacancies, schedules);

        // Act
        @class.CreateLessons(new DateOnly(2024, 08, 05), 2);

        // Assert
        @class.Lessons.First(x => x.Number == 0).Date.Should().Be(new DateOnly(2024, 08, 05));
        @class.Lessons.First(x => x.Number == 1).Date.Should().Be(new DateOnly(2024, 08, 12));
    }
}
