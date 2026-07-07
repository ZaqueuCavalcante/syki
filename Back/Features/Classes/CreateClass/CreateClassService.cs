using Syki.Back.Domain.Classes;

namespace Syki.Back.Features.Classes.CreateClass;

public class CreateClassService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<CreateClassOut, SykiError>> Create(CreateClassIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        if (data.CampusId.HasValue)
        {
            var campusOk = await ctx.Campi.AnyAsync(x => x.Id == data.CampusId && x.InstitutionId == institutionId);
            if (!campusOk) return CampusNotFound.I;
        }

        var disciplineOk = await ctx.Disciplines.AnyAsync(x => x.Id == data.DisciplineId && x.InstitutionId == institutionId);
        if (!disciplineOk) return DisciplineNotFound.I;

        if (data.TeacherId.HasValue)
        {
            var teacherOk = await ctx.Teachers.AnyAsync(x => x.Id == data.TeacherId && x.InstitutionId == institutionId);
            if (!teacherOk) return TeacherNotFound.I;

            if (data.CampusId.HasValue)
            {
                var teacherCampusOk = await ctx.TeachersCampi.AnyAsync(x => x.TeacherId == data.TeacherId && x.CampusId == data.CampusId);
                if (!teacherCampusOk) return TeacherNotAssignedToCampus.I;
            }

            var teacherDisciplineOk = await ctx.TeachersDisciplines.AnyAsync(x => x.TeacherId == data.TeacherId && x.DisciplineId == data.DisciplineId);
            if (!teacherDisciplineOk) return TeacherNotAssignedToDiscipline.I;
        }

        var period = await ctx.AcademicPeriods.FirstOrDefaultAsync(x => x.Id == data.PeriodId && x.InstitutionId == institutionId);
        if (period == null) return AcademicPeriodNotFound.I;

        var schedulesResult = data.Schedules.Select(x => (x.Day, x.Start, x.End)).ToList().ToSchedules();
        if (schedulesResult.IsError) return schedulesResult.Error;

        var @class = new Class(institutionId, data.DisciplineId, data.CampusId, data.TeacherId, period, data.Vacancies, schedulesResult.Success);
        @class.CreateLessons();

        await ctx.SaveChangesAsync(@class);

        return new CreateClassOut { Id = @class.Id };
    }
}
