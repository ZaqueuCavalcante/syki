namespace Syki.Back.Features.Cross.CreateUser;

public class SykiRoleConfig : IEntityTypeConfiguration<SykiRole>
{
    public void Configure(EntityTypeBuilder<SykiRole> role)
    {
        role.HasData(new SykiRole(AuthorizationConfigs.Adm));
        role.HasData(new SykiRole(AuthorizationConfigs.Academico));
        role.HasData(new SykiRole(AuthorizationConfigs.Professor));
        role.HasData(new SykiRole(AuthorizationConfigs.Aluno));
    }
}
