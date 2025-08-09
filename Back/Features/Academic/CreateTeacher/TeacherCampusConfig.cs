using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class TeacherCampusConfig : IEntityTypeConfiguration<TeacherCampus>
{
    public void Configure(EntityTypeBuilder<TeacherCampus> tc)
    {
        tc.ToTable("teachers__campi");

        tc.HasKey(x => new { x.SykiTeacherId, x.CampusId });

        tc.HasOne<SykiTeacher>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.SykiTeacherId);

        tc.HasOne<Campus>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.CampusId);
    }
}
