namespace Estud.Back.Features.Classes.UpdateClassTeachers;

public class UpdateClassTeachersService(EstudDbContext ctx) : IEstudService
{
    private const int MaxTeachers = 2;

    private class Validator : AbstractValidator<UpdateClassTeachersIn>
    {
        public Validator()
        {
            RuleFor(x => x.Teachers)
                .Must(x => x != null && x.Count <= MaxTeachers && x.IsAllDistinct())
                .WithError(InvalidTeachersList.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EstudSuccess, EstudError>> Update(int classId, UpdateClassTeachersIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes
            .Include(c => c.Teachers)
            .Include(c => c.Schedules)
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var teachers = await ctx.Teachers
            .Where(t => t.InstitutionId == institutionId && data.Teachers.Contains(t.Id))
            .ToListAsync();
        if (teachers.Count != data.Teachers.Count) return TeacherNotFound.I;

        var teachersInDiscipline = await ctx.TeachersDisciplines
            .CountAsync(td => data.Teachers.Contains(td.TeacherId) && td.DisciplineId == @class.DisciplineId);
        if (teachersInDiscipline != data.Teachers.Count) return TeacherNotAssignedToDiscipline.I;

        @class.Teachers = teachers;

        // Mantém a alocação de professor por horário coerente com a nova lista:
        // - 0 ou 1 professor → todos os horários apontam para ele (ou nulo);
        // - 2 professores → limpa apenas os horários que apontavam para um professor que saiu (o gestor redefine depois).
        if (data.Teachers.Count <= 1)
        {
            var teacherId = data.Teachers.Count == 1 ? data.Teachers[0] : (int?)null;
            foreach (var schedule in @class.Schedules)
                schedule.TeacherId = teacherId;
        }
        else
        {
            foreach (var schedule in @class.Schedules.Where(s => s.TeacherId == null || !data.Teachers.Contains(s.TeacherId.Value)))
                schedule.TeacherId = null;
        }

        // Conflito de agenda por professor: cada professor da turma não pode cobrir, em outra
        // turma não finalizada, um horário que choque com os que passa a cobrir nesta turma.
        foreach (var teacherId in data.Teachers)
        {
            var slotsForTeacher = @class.Schedules.Where(s => s.TeacherId == teacherId).ToList();
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

        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
