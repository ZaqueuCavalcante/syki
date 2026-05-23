namespace Syki.Back.Features.Disciplines.UpdateDiscipline;

public class UpdateDisciplineService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<UpdateDisciplineIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidDisciplineName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidDisciplineName.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateDisciplineOut, SykiError>> Update(UpdateDisciplineIn data)
    {
        if (V.Run(data, out var error)) return error;

        var discipline = await ctx.Disciplines.FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.Id);
        if (discipline == null) return DisciplineNotFound.I;

        discipline.Update(data.Name);
        await ctx.SaveChangesAsync();

        return discipline.ToUpdateDisciplineOut();
    }
}
