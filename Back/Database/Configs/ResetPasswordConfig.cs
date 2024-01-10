using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class ResetPasswordConfig : IEntityTypeConfiguration<ResetPassword>
{
    public void Configure(EntityTypeBuilder<ResetPassword> resetPassword)
    {
        resetPassword.ToTable("reset_passwords");

        resetPassword.HasKey(rp => rp.Id);
        resetPassword.Property(rp => rp.Id).ValueGeneratedNever();

        resetPassword.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(rp => rp.UserId);

        resetPassword.HasIndex(rp => rp.Token).IsUnique();
    }
}
