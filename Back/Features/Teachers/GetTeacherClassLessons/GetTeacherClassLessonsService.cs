namespace Estud.Back.Features.Teachers.GetTeacherClassLessons;

public class GetTeacherClassLessonsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherClassLessonsOut, EstudError>> Get(int classId)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.TeacherId != teacherId) return TeacherNotAssignedToClass.I;

        var lessons = await ctx.ClassLessons.AsNoTracking()
            .Where(l => l.ClassId == classId)
            .OrderBy(l => l.Number)
            .Select(l => new GetTeacherClassLessonsItemOut
            {
                Id = l.Id,
                Number = l.Number,
                Date = l.Date,
                StartAt = l.StartAt,
                EndAt = l.EndAt,
                Status = l.Status,
                PresentStudents = l.Attendances.Where(a => a.Present).Select(a => a.StudentId).ToList(),
            })
            .ToListAsync();

        return new GetTeacherClassLessonsOut { Lessons = lessons };
    }
}
