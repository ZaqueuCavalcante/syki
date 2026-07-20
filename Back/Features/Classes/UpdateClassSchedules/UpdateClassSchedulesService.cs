using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Classes.UpdateClassSchedules;

public class UpdateClassSchedulesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Update(int classId, UpdateClassSchedulesIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes
            .Include(c => c.Teachers)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.Status is ClassStatus.Started or ClassStatus.Finalized) return ClassAlreadyStarted.I;

        var schedulesResult = data.Schedules.ConvertAll(x => (x.Day, x.Start, x.End)).ToSchedules();
        if (schedulesResult.IsError) return schedulesResult.Error;
        var newSchedules = schedulesResult.Success;

        var teacherIds = @class.Teachers.Select(t => t.Id).ToList();

        // Resolve o professor de cada horário (invariante: TeacherId ∈ Class.Teachers).
        // - 0 professores: fica nulo.
        // - 1 professor: preenche automaticamente.
        // - 2 professores: obrigatório.
        for (var i = 0; i < newSchedules.Count; i++)
        {
            var teacherId = data.Schedules[i].TeacherId;
            if (teacherIds.Count == 0)
            {
                newSchedules[i].TeacherId = null;
            }
            else if (teacherIds.Count == 1)
            {
                newSchedules[i].TeacherId = teacherIds[0];
            }
            else
            {
                if (teacherId == null) return ScheduleTeacherRequired.I;
                if (!teacherIds.Contains(teacherId.Value)) return InvalidScheduleTeacher.I;
                newSchedules[i].TeacherId = teacherId;
            }
        }

        // Conflito de agenda por professor: cada professor da turma não pode cobrir, em outra
        // turma não finalizada, um horário que choque com os que ele cobre nesta turma.
        foreach (var teacherId in teacherIds)
        {
            var slotsForTeacher = newSchedules.Where(s => s.TeacherId == teacherId).ToList();
            if (slotsForTeacher.Count == 0) continue;

            var otherSchedules = await ctx.Schedules.AsNoTracking()
                .Where(s => s.ClassId != null && s.ClassId != classId
                    && s.Class!.InstitutionId == institutionId
                    && s.Class.Status != ClassStatus.Finalized
                    && (s.TeacherId == teacherId
                        || (s.TeacherId == null && s.Class.Teachers.Any(t => t.Id == teacherId))))
                .ToListAsync();

            if (slotsForTeacher.Any(ns => otherSchedules.Any(ns.Conflict)))
                return TeacherScheduleConflict.I;
        }

        ctx.Schedules.RemoveRange(@class.Schedules);
        @class.Schedules = newSchedules;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
