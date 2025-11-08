using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Features.Cross.CreateExatoUser;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Features.Cross.SendResetPasswordToken;

public class ResetPasswordTokenDbConfig : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> entity)
    {
        entity.ToTable("reset_password_tokens", "exato");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");

        entity.Property(e => e.UsedAt)
            .HasColumnType("timestamp");

        entity.HasOne<ExatoUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(o => o.ClienteId)
            .HasForeignKey(e => e.OrganizationId);

        entity.HasIndex(e => e.Token)
            .IsUnique();
    }
}
