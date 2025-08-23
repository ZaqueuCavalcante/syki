namespace Syki.Back.Features.Academic.UpdateCampus;

public class UpdateCampusService(SykiDbContext ctx) : IAcademicService
{
    private class Validator : AbstractValidator<UpdateCampusIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidCampusName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidCampusName.I);

            RuleFor(x => x.State).NotNull().WithError(InvalidBrazilState.I);
            RuleFor(x => x.State).IsInEnum().WithError(InvalidBrazilState.I);

            RuleFor(x => x.City).NotEmpty().WithError(InvalidCampusCity.I);
            RuleFor(x => x.City).MaximumLength(50).WithError(InvalidCampusCity.I);

            RuleFor(x => x.Capacity).GreaterThan(0).WithError(InvalidCampusCapacity.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateCampusOut, SykiError>> Update(Guid institutionId, UpdateCampusIn data)
    {
        if (V.Run(data, out var error)) return error;

        var campus = await ctx.Campi.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (campus == null) return CampusNotFound.I;

        campus.Update(data.Name, data.State!.Value, data.City, data.Capacity);

        await ctx.SaveChangesAsync();

        return campus.ToUpdateCampusOut();
    }
}
