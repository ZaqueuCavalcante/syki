using Estud.Back.Domain.Teachers;

namespace Estud.Back.Database.Teachers;

public class TeacherCampusDbConfig : IEntityTypeConfiguration<TeacherCampus>
{
    public void Configure(EntityTypeBuilder<TeacherCampus> entity)
    {
        entity.ToTable("teachers_campi", DbSchemas.Estud);

        entity.HasKey(e => new { e.TeacherId, e.CampusId });
    }
}
