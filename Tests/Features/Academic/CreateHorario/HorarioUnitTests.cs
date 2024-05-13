using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Tests.Unit;

public class ScheduleUnitTests
{
    [Test]
    public void Deve_criar_um_schedule_com_id()
    {
        // Arrange
        var dia = Day.Segunda;
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        // Act
        var schedule = new Schedule(dia, start, end);

        // Assert
        schedule.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Deve_criar_um_schedule_com_dia_correto()
    {
        // Arrange
        var dia = Day.Terca;
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        // Act
        var schedule = new Schedule(dia, start, end);

        // Assert
        schedule.Day.Should().Be(dia);
    }

    [Test]
    public void Deve_criar_um_schedule_com_start_correto()
    {
        // Arrange
        var dia = Day.Terca;
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        // Act
        var schedule = new Schedule(dia, start, end);

        // Assert
        schedule.Start.Should().Be(start);
    }

    [Test]
    public void Deve_criar_um_schedule_com_end_correto()
    {
        // Arrange
        var dia = Day.Terca;
        var start = Hour.H07_00;
        var end = Hour.H08_00;

        // Act
        var schedule = new Schedule(dia, start, end);

        // Assert
        schedule.End.Should().Be(end);
    }

    [Test]
    public void Nao_deve_criar_um_schedule_quando_start_e_end_forem_iguais()
    {
        // Arrange
        var dia = Day.Terca;
        var start = Hour.H07_00;
        var end = Hour.H07_00;

        // Act
        Action act = () => new Schedule(dia, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE021);
    }

    [Test]
    public void Nao_deve_criar_um_schedule_quando_end_for_menor_que_start()
    {
        // Arrange
        var dia = Day.Terca;
        var start = Hour.H10_00;
        var end = Hour.H07_00;

        // Act
        Action act = () => new Schedule(dia, start, end);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE021);
    }

    [Test]
    public void Schedules_em_dias_diferentes_nao_devem_conflitar()
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
