using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Tests.Features.Academic.CreateClass;

public class CreateClassUnitTests
{
    [Test]
    public void Should_create_class_with_correct_data()
    {
        // Arrange
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2023.2";
        const int vacancies = 40;
        var schedules = new List<Schedule>() { new(Day.Monday, Hour.H07_00, Hour.H08_00) };

        // Act
        var @class = Class.New(institutionId, disciplineId, campusId, teacherId, period, vacancies, schedules).Success;

        // Assert
        @class.Id.Should().NotBeEmpty();
        @class.InstitutionId.Should().Be(institutionId);
        @class.DisciplineId.Should().Be(disciplineId);
        @class.CampusId.Should().Be(campusId);
        @class.TeacherId.Should().Be(teacherId);
        @class.PeriodId.Should().Be(period);
        @class.Status.Should().Be(ClassStatus.OnPreEnrollment);
        @class.Vacancies.Should().Be(vacancies);
        @class.Schedules.Should().BeEquivalentTo(schedules);
    }

    [Test]
    public void Should_not_create_class_with_totally_conflicting_schedules()
    {
        // Arrange
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2023.2";
        const int vacancies = 40;
        var schedules = new List<Schedule>()
        {
            new(Day.Monday, Hour.H07_00, Hour.H08_00),
            new(Day.Monday, Hour.H07_00, Hour.H08_00),
        };

        // Act
        var result = Class.New(institutionId, campusId, disciplineId, teacherId, period, vacancies, schedules);

        // Assert
        result.ShouldBeError(new ConflictingSchedules());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ConflictingSchedules))]
    public void Should_not_create_class_with_partially_conflicting_schedules(Schedule[] schedules)
    {
        // Arrange
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2023.2";
        const int vacancies = 40;

        // Act
        var result = Class.New(institutionId, campusId, disciplineId, teacherId, period, vacancies, [.. schedules]);

        // Assert
        result.ShouldBeError(new ConflictingSchedules());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidSchedules))]
    public void Should_create_class_with_valid_schedules(Schedule[] schedules)
    {
        // Arrange
        var institutionId = Guid.CreateVersion7();
        var disciplineId = Guid.CreateVersion7();
        var campusId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string period = "2023.2";
        const int vacancies = 40;

        // Act
        var @class = Class.New(institutionId, campusId, disciplineId, teacherId, period, vacancies, [.. schedules]).Success;

        // Assert
        @class.Schedules.Should().BeEquivalentTo(schedules);
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

        var @class = Class.New(institutionId, campusId, disciplineId, teacherId, period, vacancies, schedules).Success;
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
