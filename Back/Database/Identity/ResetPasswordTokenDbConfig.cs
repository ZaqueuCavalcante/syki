using Syki.Back.Domain.Identity;

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

        entity.HasIndex(e => e.Token).IsUnique();
    }
}
