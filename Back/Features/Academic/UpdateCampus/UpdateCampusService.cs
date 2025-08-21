namespace Syki.Back.Features.Academic.UpdateCampus;

public class UpdateCampusService(SykiDbContext ctx) : IAcademicService
{
    internal class Validator : AbstractValidator<UpdateCampusIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(new InvalidCampusName());
            RuleFor(x => x.Name).MaximumLength(50).WithError(new InvalidCampusName());

            RuleFor(x => x.State).NotNull().WithError(new InvalidBrazilState());
            RuleFor(x => x.State).IsInEnum().WithError(new InvalidBrazilState());

            RuleFor(x => x.City).NotEmpty().WithError(new InvalidCampusCity());
            RuleFor(x => x.City).MaximumLength(50).WithError(new InvalidCampusCity());

            RuleFor(x => x.Capacity).GreaterThan(0).WithError(new InvalidCampusCapacity());
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CampusOut, SykiError>> Update(Guid institutionId, UpdateCampusIn data)
    {
        if (V.Run(data, out var error)) return error;

        var campus = await ctx.Campi.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (campus == null) return new CampusNotFound();

        campus.Update(data.Name, data.State!.Value, data.City, data.Capacity);

        await ctx.SaveChangesAsync();

        return campus.ToOut();
    }
}
