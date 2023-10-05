using Microsoft.EntityFrameworkCore;
using SykiRole = Syki.Back.Domain.SykiRole;
using static Syki.Back.Configs.AuthorizationConfigs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class RoleConfig : IEntityTypeConfiguration<SykiRole>
{
    public void Configure(EntityTypeBuilder<SykiRole> role)
    {
        role.HasData(new SykiRole(Adm));
        role.HasData(new SykiRole(Academico));
        role.HasData(new SykiRole(Professor));
        role.HasData(new SykiRole(Aluno));
    }
}
