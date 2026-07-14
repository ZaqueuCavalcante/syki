using Estud.Back.Cache;
using Estud.Back.Domain.Teachers;
using Estud.Back.Database.Teachers;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<EstudTeacher> Teachers { get; set; }
    public DbSet<TeacherCampus> TeachersCampi { get; set; }
    public DbSet<TeacherDiscipline> TeachersDisciplines { get; set; }

    private static void ConfigureTeachers(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EstudTeacherDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherCampusDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherDisciplineDbConfig());
    }

    public async Task<int> GetTeacherId(int institutionId, int userId)
    {
        var key = $"{CacheKeys.GetTeacherId}-{institutionId}-{userId}";

        var teacherId = await Cache.GetOrCreateAsync(
            key: key,
            state: (ctx: this, institutionId, userId),
            factory: async (state, ct) =>
            {
                return await state.ctx.Teachers.AsNoTracking()
                    .Where(x => x.InstitutionId == state.institutionId && x.UserId == state.userId)
                    .Select(x => x.Id).FirstOrDefaultAsync(ct);
            }
        );

        if (teacherId == 0) await Cache.RemoveAsync(key);

        return teacherId;
    }
}
