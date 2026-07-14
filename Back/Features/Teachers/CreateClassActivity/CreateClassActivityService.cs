using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Teachers.CreateClassActivity;

public class CreateClassActivityService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<CreateClassActivityOut, EstudError>> Create(int id, CreateClassActivityIn data)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        if (@class.TeacherId != teacherId) return TeacherNotAssignedToClass.I;

        var students = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.ClassId == id && cs.Status == StudentClassStatus.Matriculado)
            .Select(cs => cs.StudentId).ToListAsync();

        var result = ClassActivity.New(
            id,
            data.Note,
            data.Title,
            data.Description,
            data.Type,
            data.Weight,
            data.DueDate,
            data.DueHour,
            students
        );
        if (result.IsError) return result.Error;

        var noteWeight = await ctx.ClassActivities.AsNoTracking()
            .Where(a => a.ClassId == id && a.Note == data.Note)
            .SumAsync(a => a.Weight);
        if (noteWeight + data.Weight > 100) return InvalidClassActivityWeight.I;

        var activity = result.Success;
        await ctx.SaveChangesAsync(activity);

        return new CreateClassActivityOut { Id = activity.Id };
    }
}
