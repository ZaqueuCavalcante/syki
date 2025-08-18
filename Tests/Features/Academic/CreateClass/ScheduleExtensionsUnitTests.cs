using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Tests.Features.Academic.CreateClass;

public class ScheduleExtensionsUnitTests
{
    [Test]
    public void Should_convert_valid_schedules()
    {
        // Arrange
        var schedulesIn = new List<ScheduleIn>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        // Act
        var result = schedulesIn.ToSchedules();

        // Assert
        result.ShouldBeSuccess();
        result.Success.Should().BeEquivalentTo([new { Day = Day.Monday, StartAt = Hour.H07_00, EndAt = Hour.H08_00 }]);
    }

    [Test]
    public void Should_not_create_class_with_totally_conflicting_schedules()
    {
        // Arrange
        var schedulesIn = new List<ScheduleIn>()
        {
            new(Day.Monday, Hour.H07_00, Hour.H08_00),
            new(Day.Monday, Hour.H07_00, Hour.H08_00),
        };

        // Act
        var result = schedulesIn.ToSchedules();

        // Assert
        result.ShouldBeError(new ConflictingSchedules());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ConflictingSchedules))]
    public void Should_not_create_class_with_partially_conflicting_schedules(ScheduleIn[] schedulesIn)
    {
        // Arrange / Act
        var result = schedulesIn.ToList().ToSchedules();

        // Assert
        result.ShouldBeError(new ConflictingSchedules());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidSchedules))]
    public void Should_create_class_with_valid_schedules(ScheduleIn[] schedulesIn)
    {
        // Arrange / Act
        var result = schedulesIn.ToList().ToSchedules();

        // Assert
        result.Success.Should().BeEquivalentTo(schedulesIn.Select(x => new { x.Day, StartAt = x.Start, EndAt = x.End }));
    }

    [Test]
    public void Should_convert_class_to_out()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2023.2";
        const int vacancies = 40;

        var discipline = new Discipline(institutionId, "Banco de Dados");
        var teacher = new SykiTeacher(userId, institutionId, "Chico Science");
        var schedules = new List<Schedule>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        var @class = new Class(institutionId, campusId, disciplineId, teacherId, period, vacancies, schedules);
        @class.Discipline = discipline;
        @class.Teacher = teacher;

        // Act
        var classOut = @class.ToOut();

        // Assert
        classOut.Id.Should().Be(@class.Id);
        classOut.Discipline.Should().Be(@class.Discipline.Name);
        classOut.Teacher.Should().Be(@class.Teacher.Name);
        classOut.Period.Should().Be(@class.PeriodId);
        classOut.Schedules.Should().HaveCount(1);
    }
}
