using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class ResetPasswordTokenDbConfig : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> entity)
    {
        entity.ToTable("reset_password_tokens", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.HasOne<EstudUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasIndex(e => e.Token).IsUnique();
    }
}
