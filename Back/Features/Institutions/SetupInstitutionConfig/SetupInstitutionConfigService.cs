namespace Estud.Back.Features.Institutions.SetupInstitutionConfig;

public class SetupInstitutionConfigService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<SetupInstitutionConfigIn>
    {
        public Validator()
        {
            RuleFor(x => x.NoteLimit).InclusiveBetween(0.00M, 10.00M).WithError(InvalidNoteLimit.I);

            RuleFor(x => x.FrequencyLimit).InclusiveBetween(0.00M, 100.00M).WithError(InvalidFrequencyLimit.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<SetupInstitutionConfigOut, EstudError>> Setup(SetupInstitutionConfigIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var config = await ctx.InstitutionConfigs.FirstAsync(x => x.InstitutionId == institutionId);
        config.Setup(data.NoteLimit, data.FrequencyLimit);

        await ctx.SaveChangesAsync();

        return config.ToSetupInstitutionConfigOut();
    }
}
