using Estud.Back.Cache;
using Estud.Back.Domain.Parents;
using Estud.Back.Database.Parents;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<EstudParent> Parents { get; set; }
    public DbSet<ParentStudent> ParentStudents { get; set; }

    private static void ConfigureParents(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EstudParentDbConfig());
        modelBuilder.ApplyConfiguration(new ParentStudentDbConfig());
    }

    public async Task<int> GetParentId(int institutionId, int userId)
    {
        var key = $"{CacheKeys.GetParentId}-{institutionId}-{userId}";

        var parentId = await Cache.GetOrCreateAsync(
            key: key,
            state: (ctx: this, institutionId, userId),
            factory: async (state, ct) =>
            {
                return await state.ctx.Parents.AsNoTracking()
                    .Where(x => x.InstitutionId == state.institutionId && x.UserId == state.userId)
                    .Select(x => x.Id).FirstOrDefaultAsync(ct);
            }
        );

        if (parentId == 0) await Cache.RemoveAsync(key);

        return parentId;
    }
}
