using Syki.Back.Domain.Teachers;

namespace Syki.Back.Database.Teachers;

public class TeacherCampusDbConfig : IEntityTypeConfiguration<TeacherCampus>
{
    public void Configure(EntityTypeBuilder<TeacherCampus> entity)
    {
        entity.ToTable("teachers_campi", DbSchemas.Syki);

        entity.HasKey(e => new { e.TeacherId, e.CampusId });
    }
}
