using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class ResetPasswordTokenConfig : IEntityTypeConfiguration<ResetPasswordToken>
{
    public void Configure(EntityTypeBuilder<ResetPasswordToken> resetPassword)
    {
        resetPassword.ToTable("reset_password_tokens");

        resetPassword.HasKey(rp => rp.Id);
        resetPassword.Property(rp => rp.Id).ValueGeneratedNever();

        resetPassword.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(rp => rp.UserId);

        resetPassword.HasIndex(rp => rp.Token).IsUnique();
    }
}
