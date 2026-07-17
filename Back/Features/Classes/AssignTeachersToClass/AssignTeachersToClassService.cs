namespace Estud.Back.Features.Classes.AssignTeachersToClass;

public class AssignTeachersToClassService(EstudDbContext ctx) : IEstudService
{
    private const int MaxTeachers = 2;

    private class Validator : AbstractValidator<AssignTeachersToClassIn>
    {
        public Validator()
        {
            RuleFor(x => x.Teachers)
                .Must(x => x != null && x.Count <= MaxTeachers && x.IsAllDistinct())
                .WithError(InvalidTeachersList.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EstudSuccess, EstudError>> Assign(int classId, AssignTeachersToClassIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var @class = await ctx.Classes.Include(c => c.Teachers)
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
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
