using Syki.Back.Features.Academic.CreateCourseOffering;

namespace Syki.Back.Features.Academic.CreateCampus;

public class CampusConfig : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> campus)
    {
        campus.ToTable("campi");

        campus.HasKey(c => c.Id);
        campus.Property(c => c.Id).ValueGeneratedNever();

        campus.Property(c => c.Name);
        campus.Property(c => c.City);

        campus.HasMany<CourseOffering>()
            .WithOne(o => o.Campus)
            .HasForeignKey(o => o.CampusId);
    }
}
