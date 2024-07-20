using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Tests.Features.Academic.CreateSchedule;

public class CreateScheduleUnitTests
{
    [Test]
    public void Should_create_schedule_with_correct_data()
    {
        // Arrange
        var day = Day.Segunda;
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        // Act
        var schedule = new Schedule(day, start, end);

        // Assert
        schedule.Id.Should().NotBeEmpty();
        schedule.Day.Should().Be(day);
        schedule.StartAt.Should().Be(start);
        schedule.EndAt.Should().Be(end);
    }

    [Test]
    public void Nao_deve_criar_um_schedule_quando_start_e_end_forem_iguais()
    {
        // Arrange
        var day = Day.Terca;
        var start = Hour.H07_00;
        var end = Hour.H07_00;

        // Act
        var result = Schedule.New(day, start, end);

        // Assert
        result.ShouldBeError(new InvalidSchedule());
    }

    [Test]
    public void Nao_deve_criar_um_schedule_quando_end_for_menor_que_start()
    {
        // Arrange
        var day = Day.Terca;
        var start = Hour.H10_00;
        var end = Hour.H07_00;

        // Act
        var result = Schedule.New(day, start, end);

        // Assert
        result.ShouldBeError(new InvalidSchedule());
    }

    [Test]
    public void Schedules_em_days_diferentes_nao_devem_conflitar()
    {
        // Arrange
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        var scheduleA = new Schedule(Day.Segunda, start, end);
        var scheduleB = new Schedule(Day.Terca, start, end);

        // Act
        var result = scheduleA.Conflict(scheduleB);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Schedules_validos_nao_devem_conflitar()
    {
        // Arrange
        var scheduleA = new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00);
        var scheduleB = new Schedule(Day.Segunda, Hour.H08_00, Hour.H08_30);

        // Act
        var result = scheduleA.Conflict(scheduleB);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Schedules_exatamente_iguais_devem_conflitar()
    {
        // Arrange
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        var scheduleA = new Schedule(Day.Segunda, start, end);
        var scheduleB = new Schedule(Day.Segunda, start, end);

        // Act
        var result = scheduleA.Conflict(scheduleB);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Schedules_parcialmente_iguais_devem_conflitar()
    {
        // Arrange
        var scheduleA = new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00);
        var scheduleB = new Schedule(Day.Segunda, Hour.H07_30, Hour.H08_30);

        // Act
        var result = scheduleA.Conflict(scheduleB);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Schedules_cujo_segundo_esta_contido_no_primeiro_devem_conflitar()
    {
        // Arrange
        var scheduleA = new Schedule(Day.Segunda, Hour.H07_00, Hour.H08_00);
        var scheduleB = new Schedule(Day.Segunda, Hour.H07_30, Hour.H07_45);

        // Act
        var result = scheduleA.Conflict(scheduleB);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Schedules_cujo_primeiro_esta_contido_no_segundo_devem_conflitar()
    {
        // Arrange
        var scheduleA = new Schedule(Day.Segunda, Hour.H10_00, Hour.H11_00);
        var scheduleB = new Schedule(Day.Segunda, Hour.H09_30, Hour.H12_15);

        // Act
        var result = scheduleA.Conflict(scheduleB);

        // Assert
        result.Should().BeTrue();
    }
}
