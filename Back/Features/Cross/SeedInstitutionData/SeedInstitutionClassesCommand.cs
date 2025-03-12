using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Back.Features.Cross.SeedInstitutionData;

[CommandDescription("Realizar seed de turmas da instituição")]
public record SeedInstitutionClassesCommand(Guid InstitutionId) : ICommand;

public class SeedInstitutionClassesCommandHandler(
    SykiDbContext ctx,
    CreateClassService createClassService) : ICommandHandler<SeedInstitutionClassesCommand>
{
    public async Task Handle(Guid commandId, SeedInstitutionClassesCommand command)
    {
        var id = command.InstitutionId;
        var academicPeriod = $"{DateTime.Now.Year}.1";

        var adsCourseCurriculumId = await ctx.CourseCurriculums
            .Where(x => x.InstitutionId == id && x.Name.Equals("Grade ADS 1.0"))
            .Select(x => x.Id).FirstAsync();
        var adsDisciplines = await ctx.CourseCurriculumDisciplines
            .Where(x => x.CourseCurriculumId == adsCourseCurriculumId && x.Period == 1)
            .ToListAsync();

        var teachers = await ctx.Teachers.Where(x => x.InstitutionId == id).Select(x => x.Id).ToListAsync();

        for (int i = 0; i < 6; i++)
        {
            await createClassService.Create(id, new()
            {
                DisciplineId = adsDisciplines[i].DisciplineId,
                TeacherId = teachers.PickRandom(),
                Period = academicPeriod,
                Vacancies = new List<int>{25, 30, 35}.PickRandom(),
                Schedules = [new() { Day = (Day)i, Start = Hour.H19_00, End = Hour.H22_00 }]
            });
        }

        ctx.AddCommand(id, new SeedInstitutionEnrollmentsCommand(id, adsCourseCurriculumId), parentId: commandId);
    }
}
