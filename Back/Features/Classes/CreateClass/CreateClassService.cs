using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Classes.CreateClass;

public class CreateClassService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<CreateClassIn>
    {
        public Validator()
        {
            RuleFor(x => x.Vacancies).GreaterThanOrEqualTo(0).WithError(InvalidClassVacancies.I);
            RuleFor(x => x.Vacancies).LessThanOrEqualTo(100).WithError(InvalidClassVacancies.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateClassOut, EstudError>> Create(CreateClassIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var disciplineOk = await ctx.Disciplines.AnyAsync(x => x.Id == data.DisciplineId && x.InstitutionId == institutionId);
        if (!disciplineOk) return DisciplineNotFound.I;

        var periodOk = await ctx.AcademicPeriods.AnyAsync(x => x.Id == data.PeriodId && x.InstitutionId == institutionId);
        if (!periodOk) return AcademicPeriodNotFound.I;

        if (data.CampusId.HasValue)
        {
            var campusOk = await ctx.Campi.AnyAsync(x => x.Id == data.CampusId && x.InstitutionId == institutionId);
            if (!campusOk) return CampusNotFound.I;
        }

        var @class = new Class(institutionId, data.DisciplineId, data.PeriodId, data.Vacancies, data.CampusId);
        await ctx.SaveChangesAsync(@class);

        return new CreateClassOut { Id = @class.Id };
    }
}
