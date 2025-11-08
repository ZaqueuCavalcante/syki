using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoUserDbConfig : IEntityTypeConfiguration<ExatoUser>
{
    public void Configure(EntityTypeBuilder<ExatoUser> entity)
    {
        entity.ToTable("users", "exato");

        entity.Ignore(e => e.Role);

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
    }
}
