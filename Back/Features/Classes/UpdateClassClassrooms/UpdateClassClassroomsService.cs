namespace Estud.Back.Features.Classes.UpdateClassClassrooms;

public class UpdateClassClassroomsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Update(int classId, UpdateClassClassroomsIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.Status is ClassStatus.Started or ClassStatus.Finalized) return ClassAlreadyStarted.I;

        // Cada item precisa apontar para um horário existente da turma (chave natural: dia + início + fim).
        var scheduleByKey = @class.Schedules.ToDictionary(s => (s.Day, s.Start, s.End));
        foreach (var item in data.Classrooms)
            if (!scheduleByKey.ContainsKey((item.Day, item.Start, item.End)))
                return ScheduleNotFound.I;

        var classroomIds = data.Classrooms.Select(x => x.ClassroomId).Distinct().ToList();
        var classrooms = await ctx.Classrooms
            .Where(c => c.InstitutionId == institutionId && classroomIds.Contains(c.Id))
            .ToListAsync();
        if (classrooms.Count != classroomIds.Count) return ClassroomNotFound.I;

        // Sala e turma precisam estar no mesmo campus e a sala precisa comportar as vagas da turma.
        foreach (var classroom in classrooms)
        {
            if (@class.CampusId != classroom.CampusId) return ClassAndClassroomDifferentCampus.I;
            if (classroom.Capacity < @class.Vacancies) return ClassroomCapacityExceeded.I;
        }

        // Conflito de sala: os horários que a turma quer usar numa sala não podem chocar com os
        // horários de outra turma não finalizada já alocada na mesma sala.
        foreach (var classroom in classrooms)
        {
            var slots = data.Classrooms
                .Where(x => x.ClassroomId == classroom.Id)
                .Select(x => scheduleByKey[(x.Day, x.Start, x.End)])
                .ToList();

            var otherSchedules = await ctx.Schedules.AsNoTracking()
                .Where(s => s.ClassroomId == classroom.Id
                    && s.ClassId != null && s.ClassId != classId
                    && s.Class!.Status != ClassStatus.Finalized)
                .ToListAsync();

            if (slots.Any(slot => otherSchedules.Any(slot.Conflict)))
                return ClassroomScheduleConflict.I;
        }

        // Replace-all: limpa a sala de todos os horários da turma e aplica os novos vínculos.
        foreach (var schedule in @class.Schedules)
            schedule.ClassroomId = null;

        foreach (var item in data.Classrooms)
            scheduleByKey[(item.Day, item.Start, item.End)].ClassroomId = item.ClassroomId;

        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
