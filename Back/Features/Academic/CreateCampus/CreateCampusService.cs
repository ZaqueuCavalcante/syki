namespace Syki.Back.Features.Academic.CreateCampus;

public class CreateCampusService(SykiDbContext ctx) : IAcademicService
{
    private class Validator : AbstractValidator<CreateCampusIn>
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

    public async Task<OneOf<CreateCampusOut, SykiError>> Create(CreateCampusIn data)
    {
        if (V.Run(data, out var error)) return error;

        var campus = new Campus(ctx.InstitutionId, data.Name, data.State!.Value, data.City, data.Capacity);
        await ctx.SaveChangesAsync(campus);

        return campus.ToCreateCampusOut();
    }
}
