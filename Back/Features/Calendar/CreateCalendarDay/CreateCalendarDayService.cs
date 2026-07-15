using Estud.Back.Domain.Calendar;

namespace Estud.Back.Features.Calendar.CreateCalendarDay;

public class CreateCalendarDayService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<CreateCalendarDayIn>
    {
        public Validator()
        {
            RuleFor(x => x.Date.Year).InclusiveBetween(1970, 2070).WithError(InvalidCalendarDayDate.I);

            RuleFor(x => x.DayType).NotNull().WithError(InvalidCalendarDayType.I);
            RuleFor(x => x.DayType).IsInEnum().WithError(InvalidCalendarDayType.I);

            RuleFor(x => x.Description).MaximumLength(100).WithError(InvalidCalendarDayDescription.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateCalendarDayOut, EstudError>> Create(CreateCalendarDayIn data)
    {
        if (V.Run(data, out var error)) return error;
        var institutionId = ctx.RequestUser.InstitutionId;

        var date = DateOnly.FromDateTime(data.Date);

        var exists = await ctx.CalendarDays.AnyAsync(x => x.InstitutionId == institutionId && x.Date == date);
        if (exists) return CalendarDayAlreadyExists.I;

        var day = new CalendarDay(institutionId, date, data.DayType!.Value, data.Description);
        await ctx.SaveChangesAsync(day);

        return new CreateCalendarDayOut { Id = day.Id };
    }
}
