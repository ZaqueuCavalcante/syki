using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Activities;
using Estud.Back.Domain.Institutions;

namespace Estud.Back.Database.Activities;

public class UserActivityDbConfig : IEntityTypeConfiguration<UserActivity>
{
    public void Configure(EntityTypeBuilder<UserActivity> entity)
    {
        entity.ToTable("user_activities", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Metadata)
            .HasColumnType("jsonb");

        entity.HasOne<EstudUser>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.InstitutionId);
    }
}
