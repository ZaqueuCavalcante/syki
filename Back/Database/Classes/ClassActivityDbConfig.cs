using Estud.Back.Domain.Classes;

namespace Estud.Back.Database.Classes;

public class ClassActivityDbConfig : IEntityTypeConfiguration<ClassActivity>
{
    public void Configure(EntityTypeBuilder<ClassActivity> entity)
    {
        entity.ToTable("class_activities", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Works)
            .WithOne()
            .HasForeignKey(caw => caw.ClassActivityId);
    }
}
