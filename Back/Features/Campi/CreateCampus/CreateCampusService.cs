using Estud.Back.Domain.Campi;

namespace Estud.Back.Features.Campi.CreateCampus;

public class CreateCampusService(EstudDbContext ctx) : IEstudService
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
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateCampusOut, EstudError>> Create(CreateCampusIn data)
    {
        if (V.Run(data, out var error)) return error;

        var campus = new Campus(ctx.RequestUser.InstitutionId, data.Name, data.State!.Value, data.City);
        await ctx.SaveChangesAsync(campus);

        return new CreateCampusOut { Id = campus.Id };
    }
}
