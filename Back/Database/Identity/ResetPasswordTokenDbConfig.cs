using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Identity;

public class ResetPasswordTokenDbConfig : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> entity)
    {
        entity.ToTable("reset_password_tokens", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.InstitutionId);

        entity.HasIndex(e => e.Token).IsUnique();
    }
}
