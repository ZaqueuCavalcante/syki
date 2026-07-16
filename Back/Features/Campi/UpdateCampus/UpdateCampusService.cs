namespace Estud.Back.Features.Campi.UpdateCampus;

public class UpdateCampusService(EstudDbContext ctx) : IEstudService
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
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateCampusOut, EstudError>> Update(UpdateCampusIn data)
    {
        if (V.Run(data, out var error)) return error;

        var campus = await ctx.Campi.FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.Id);
        if (campus == null) return CampusNotFound.I;

        campus.Update(data.Name, data.State!.Value, data.City);

        await ctx.SaveChangesAsync();

        return campus.ToUpdateCampusOut();
    }
}
