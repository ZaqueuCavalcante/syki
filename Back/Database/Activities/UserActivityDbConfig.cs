using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Activities;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Activities;

public class UserActivityDbConfig : IEntityTypeConfiguration<UserActivity>
{
    public void Configure(EntityTypeBuilder<UserActivity> entity)
    {
        entity.ToTable("user_activities", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Metadata)
            .HasColumnType("jsonb");

        entity.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.InstitutionId);
    }
}
