namespace Syki.Back.Features.Student.GetStudentActivities;

public class GetStudentActivitiesService(SykiDbContext ctx) : IStudentService
{
    public async Task<List<StudentActivityOut>> Get(Guid userId)
    {
        var classesStudents = await ctx.ClassesStudents.AsNoTracking()
            .Where(x => x.SykiStudentId == userId).ToListAsync();

        var classesIds = classesStudents.ConvertAll(x => x.ClassId);
        var classes = await ctx.Classes.AsNoTracking()
            .Include(x => x.Activities)
            .Where(x => classesIds.Contains(x.Id))
            .ToListAsync();

        var result = new List<StudentActivityOut>();

        foreach (var @class in classes)
        {
            foreach (var activity in @class.Activities)
            {
                result.Add(activity.ToStudentActivityOut());
            }
        }

        return result.OrderByDescending(x => x.CreatedAt).ToList();
    }
}
