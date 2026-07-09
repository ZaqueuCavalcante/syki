using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class UserSocialLoginDbConfig : IEntityTypeConfiguration<UserSocialLogin>
{
    public void Configure(EntityTypeBuilder<UserSocialLogin> entity)
    {
        entity.ToTable("user_social_logins", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasIndex(e => new { e.Provider, e.ProviderKey })
            .IsUnique();

        entity.HasIndex(e => new { e.Provider, e.UserId })
            .IsUnique();

        entity.HasIndex(e => new { e.Provider, e.Email });
    }
}
