using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.CreateUser;

public class SykiRoleConfig : IEntityTypeConfiguration<SykiRole>
{
    public void Configure(EntityTypeBuilder<SykiRole> role)
    {
        role.HasData(new SykiRole(Adm));
        role.HasData(new SykiRole(Academico));
        role.HasData(new SykiRole(Professor));
        role.HasData(new SykiRole(Aluno));
    }
}
