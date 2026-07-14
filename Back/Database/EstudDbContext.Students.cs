using Estud.Back.Cache;
using Estud.Back.Domain.Students;
using Estud.Back.Database.Students;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<EstudStudent> Students { get; set; }
    public DbSet<StudentCourseEnrollment> StudentCourseEnrollments { get; set; }

    private static void ConfigureStudents(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EstudStudentDbConfig());
        modelBuilder.ApplyConfiguration(new StudentClassNoteDbConfig());
        modelBuilder.ApplyConfiguration(new StudentCourseEnrollmentDbConfig());
    }

    public async Task<int> GetStudentId(int institutionId, int userId)
    {
        var key = $"{CacheKeys.GetStudentId}-{institutionId}-{userId}";

        var studentId = await Cache.GetOrCreateAsync(
            key: key,
            state: (ctx: this, institutionId, userId),
            factory: async (state, ct) =>
            {
                return await state.ctx.Students.AsNoTracking()
                    .Where(x => x.InstitutionId == state.institutionId && x.UserId == state.userId)
                    .Select(x => x.Id).FirstOrDefaultAsync(ct);
            }
        );

        if (studentId == 0) await Cache.RemoveAsync(key);

        return studentId;
    }
}
