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
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2023.2";
        var schedules = new List<Schedule>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        // Act
        var @class = new Class(institutionId, disciplineId, teacherId, period, schedules);

        // Assert
        @class.Id.Should().NotBeEmpty();
        @class.InstitutionId.Should().Be(institutionId);
        @class.DisciplineId.Should().Be(disciplineId);
        @class.TeacherId.Should().Be(teacherId);
        @class.Period.Should().Be(period);
        @class.Schedules.Should().BeEquivalentTo(schedules);
    }

    [Test]
    public void Should_not_create_class_with_totally_conflicting_schedules()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2023.2";
        var schedules = new List<Schedule>()
        {
            new(Day.Segunda, Hour.H07_00, Hour.H08_00),
            new(Day.Segunda, Hour.H07_00, Hour.H08_00),
        };

        // Act
        Action act = () => new Class(institutionId, disciplineId, teacherId, period, schedules);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE022);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ConflictingSchedules))]
    public void Should_not_create_class_with_partially_conflicting_schedules(List<Schedule> schedules)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        Action act = () => new Class(institutionId, disciplineId, teacherId, period, schedules);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE022);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidSchedules))]
    public void Should_create_class_with_valid_schedules(List<Schedule> schedules)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2023.2";

        // Act
        var @class = new Class(institutionId, disciplineId, teacherId, period, schedules);

        // Assert
        @class.Schedules.Should().BeEquivalentTo(schedules);
    }

    [Test]
    public void Should_convert_class_to_out()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var disciplineId = Guid.NewGuid();
        var teacherId = Guid.NewGuid();
        const string period = "2023.2";

        var discipline = new Discipline(institutionId, "Banco de Dados");
        var teacher = new Teacher(userId, institutionId, "Chico Science");
        var schedules = new List<Schedule>() { new(Day.Segunda, Hour.H07_00, Hour.H08_00) };

        var @class = new Class(institutionId, disciplineId, teacherId, period, schedules)
        {
            Discipline = discipline,
            Teacher = teacher,
        };

        // Act
        var classOut = @class.ToOut();

        // Assert
        classOut.Id.Should().Be(@class.Id);
        classOut.Discipline.Should().Be(@class.Discipline.Name);
        classOut.Teacher.Should().Be(@class.Teacher.Name);
        classOut.Period.Should().Be(@class.Period);
        classOut.Schedules.Should().HaveCount(1);
    }
}
