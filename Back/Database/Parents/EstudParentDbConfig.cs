using Estud.Back.Domain.Parents;
using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Parents;

public class EstudParentDbConfig : IEntityTypeConfiguration<EstudParent>
{
    public void Configure(EntityTypeBuilder<EstudParent> entity)
    {
        entity.ToTable("parents", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithOne()
            .HasPrincipalKey<EstudUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<EstudParent>(e => new { e.InstitutionId, e.UserId });
    }
}
