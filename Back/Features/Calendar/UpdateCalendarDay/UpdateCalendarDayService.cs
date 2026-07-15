namespace Estud.Back.Features.Calendar.UpdateCalendarDay;

public class UpdateCalendarDayService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<UpdateCalendarDayIn>
    {
        public Validator()
        {
            RuleFor(x => x.DayType).NotNull().WithError(InvalidCalendarDayType.I);
            RuleFor(x => x.DayType).IsInEnum().WithError(InvalidCalendarDayType.I);

            RuleFor(x => x.Description).MaximumLength(100).WithError(InvalidCalendarDayDescription.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateCalendarDayOut, EstudError>> Update(UpdateCalendarDayIn data)
    {
        if (V.Run(data, out var error)) return error;
        var institutionId = ctx.RequestUser.InstitutionId;

        var day = await ctx.CalendarDays.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (day == null) return CalendarDayNotFound.I;

        day.Update(data.DayType!.Value, data.Description);

        await ctx.SaveChangesAsync();

        return day.ToUpdateCalendarDayOut();
    }
}
