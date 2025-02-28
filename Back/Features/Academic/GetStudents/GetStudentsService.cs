namespace Syki.Back.Features.Academic.GetStudents;

public class GetStudentsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<StudentOut>> Get(Guid institutionId, HybridCache cache)
    {
        return await cache.GetOrCreateAsync(
            key: $"students:{institutionId}",
            state: (ctx, institutionId),
            factory: async (state, _) =>
            {
                var data = await state.ctx.Students.AsNoTracking()
                    .AsSplitQuery()
                    .Include(a => a.User)
                    .Include(a => a.CourseOffering)
                        .ThenInclude(o => o.Course)
                    .Where(a => a.InstitutionId == state.institutionId)
                    .ToListAsync();
                return data.ConvertAll(c => c.ToOut());
            }
        );
    }
}
