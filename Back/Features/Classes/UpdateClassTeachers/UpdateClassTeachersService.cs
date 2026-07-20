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
            var only = data.Teachers.Count == 1 ? data.Teachers[0] : (int?)null;
            foreach (var schedule in @class.Schedules)
                schedule.TeacherId = only;
        }
        else
        {
            foreach (var schedule in @class.Schedules.Where(s => s.TeacherId == null || !data.Teachers.Contains(s.TeacherId.Value)))
                schedule.TeacherId = null;
        }

        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
