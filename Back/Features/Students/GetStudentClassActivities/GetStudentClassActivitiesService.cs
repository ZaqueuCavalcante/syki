namespace Estud.Back.Features.Students.GetStudentClassActivities;

public class GetStudentClassActivitiesService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetStudentClassActivitiesOut, EstudError>> Get(int id)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.GetStudentId(institutionId, userId);

        var classOk = await ctx.Classes.AsNoTracking().AnyAsync(c => c.Id == id && c.InstitutionId == institutionId);
        if (!classOk) return ClassNotFound.I;

        var enrolled = await ctx.ClassStudents.AsNoTracking().AnyAsync(cs => cs.ClassId == id && cs.StudentId == studentId);
        if (!enrolled) return StudentNotEnrolledInClass.I;

        var activities = await ctx.ClassActivities.AsNoTracking()
            .Where(a => a.ClassId == id)
            .OrderBy(a => a.Note)
            .ThenBy(a => a.CreatedAt)
            .Select(a => new
            {
                Activity = a,
                Work = a.Works.FirstOrDefault(w => w.StudentId == studentId),
            })
            .ToListAsync();

        return new GetStudentClassActivitiesOut
        {
            Activities = activities.ConvertAll(x => x.Activity.ToGetStudentClassActivitiesItemOut(x.Work)),
        };
    }
}
